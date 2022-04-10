using System;
using System.Collections.Generic;
using rock.Core.Domains.Threads;
using rock.Core.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Thread = rock.Core.Domains.Threads.Thread;
using rock.Core.Errors;
using rock.Core.Services.Common;
namespace rock.Core.Services.Threads
{
  public class ThreadService : IThreadService
  {
    #region Fields
    private readonly IWorkContext workContext;
    private readonly IRepository<Thread> threadRepository;
    private readonly IRepository<ThreadActivity> threadActivityRepository;
    private readonly IErrorFactory errorFactory;
    #endregion
    #region Constractor
    public ThreadService(
      IWorkContext workContext,
      IRepository<Thread> threadRepository,
      IRepository<ThreadActivity> threadActivityRepository,
      IErrorFactory errorFactory)
    {
      this.workContext = workContext;
      this.threadRepository = threadRepository;
      this.threadActivityRepository = threadActivityRepository;
      this.errorFactory = errorFactory;
    }
    #endregion
    #region Thread
    public async Task<Thread> newThread(CancellationToken cancellationToken)
    {
      var thread = new Thread();
      await threadRepository.AddAsync(thread, cancellationToken);
      return thread;
    }
    public async Task DeleteThread(Thread thread, CancellationToken cancellationToken)
    {
      await DeleteThreadActiviesByThreadId(threadId: thread.Id, cancellationToken: cancellationToken);
      await threadRepository.DeleteAsync(thread, cancellationToken);
    }
    public async Task<Thread> GetThreadById(int id, CancellationToken cancellationToken, IInclude<Thread> include = null)
    {
      return await threadRepository.GetAsync(predicate: x => x.Id == id && x.DeletedAt == null,
                                  cancellationToken: cancellationToken,
                                  include: include);
    }
    #endregion
    #region ThreadAcitvity
    public async Task<ThreadActivity> InsertThreadActivity(ThreadActivity threadActivity,
                                                           Thread thread,
                                                           CancellationToken cancellationToken,
                                                           ThreadActivity reference = null)
    {
      threadActivity.UserId = workContext.GetCurrentUserId();
      threadActivity.Reference = reference;
      threadActivity.Thread = thread;
      threadActivity.CreatedAt = DateTime.UtcNow;
      await threadActivityRepository.AddAsync(entity: threadActivity,
                                              cancellationToken: cancellationToken);
      if (threadActivity.Type == ThreadActivityType.Like ||
          threadActivity.Type == ThreadActivityType.Tag ||
          threadActivity.Type == ThreadActivityType.Visit ||
          threadActivity.Type == ThreadActivityType.Rate)
      {
        await PublishThreadActivity(threadActivity: threadActivity,
                                    cancellationToken: cancellationToken);
      }
      return threadActivity;
    }
    public async Task PublishThreadActivity(ThreadActivity threadActivity, CancellationToken cancellationToken)
    {
      threadActivity.PublisherId = workContext.GetCurrentUserId();
      threadActivity.PublishAt = DateTime.UtcNow;
      await threadActivityRepository.UpdateAsync(threadActivity, cancellationToken);
    }
    public async Task UpdateThreadActivity(ThreadActivity threadActivity, CancellationToken cancellationToken, bool checkOwner = false)
    {
      threadActivity.UpdatedAt = DateTime.UtcNow;
      await threadActivityRepository.UpdateAsync(threadActivity, cancellationToken);
    }
    public async Task DeleteThreadActivity(ThreadActivity threadActivity, CancellationToken cancellationToken, bool checkOwner = false)
    {
      if (checkOwner)
      {
        var currentUserId = workContext.GetCurrentUserId();
        if (threadActivity.UserId != currentUserId)
          throw this.errorFactory.AccessDenied();
      }
      await threadActivityRepository.DeleteAsync(threadActivity, cancellationToken);
    }
    public async Task DeleteThreadActiviesByThreadId(int threadId, CancellationToken cancellationToken)
    {
      await threadActivityRepository.DeleteAsync(predicate: x => x.ThreadId == threadId,
                                                     cancellationToken: cancellationToken);
    }
    public async Task<ThreadActivity> GetThreadActivityById(int id,
                                                            CancellationToken cancellationToken,
                                                            int? threadId = null,
                                                            bool showUnPublished = true,
                                                            IInclude<ThreadActivity> include = null)
    {
      return await threadActivityRepository.GetAsync(predicate: x => x.DeletedAt == null && x.Id == id && (threadId == null || x.ThreadId == threadId),
                                                     cancellationToken: cancellationToken,
                                                     include: include);
    }
    public IQueryable<ThreadActivity> GetThreadActivities(bool showUnPublished = true, IInclude<ThreadActivity> include = null)
    {
      var query = threadActivityRepository.GetQuery(include).Where(x => x.DeletedAt == null);
      if (showUnPublished == false)
        query = query.Where(x => x.PublishAt != null);
      return query;
    }
    public IQueryable<ThreadActivity> GetUserThreadActivities(bool showUnPublished = true, IInclude<ThreadActivity> include = null)
    {
      int? currentUserId = null;
      if (workContext.IsAuthenticated())
        currentUserId = workContext.GetCurrentUserId();
      return GetThreadActivities(showUnPublished: showUnPublished,
                                 include: include)
                                 .Where(x => x.UserId == currentUserId);
    }
    public async Task<ThreadActivity> Like(Thread thread, CancellationToken cancellationToken, ThreadActivity activity = null)
    {
      await Unlike(thread: thread,
                   activity: activity,
                   cancellationToken: cancellationToken);
      var likeActivity = new ThreadActivity();
      likeActivity.Type = ThreadActivityType.Like;
      await InsertThreadActivity(threadActivity: likeActivity,
                           thread: thread,
                           cancellationToken: cancellationToken,
                           reference: activity);
      return likeActivity;
    }
    public async Task Unlike(Thread thread, CancellationToken cancellationToken, ThreadActivity activity = null)
    {
      var userId = this.workContext.GetCurrentUserId();
      var activityId = activity?.Id;
      var like = await threadActivityRepository.GetAsync(predicate: x => x.UserId == userId &&
                  x.DeletedAt == null &&
                  x.ThreadId == thread.Id &&
                  (activityId == null || x.ReferenceId == activity.Id) &&
                  x.Type == ThreadActivityType.Like,
                  cancellationToken: cancellationToken);
      if (like != null)
        await DeleteThreadActivity(like, cancellationToken);
    }
    public async Task<ThreadActivity> Visit(Thread thread, CancellationToken cancellationToken)
    {
      var userId = this.workContext.GetCurrentUserId();
      var visit = await threadActivityRepository.GetAsync(predicate: x => x.UserId == userId &&
                  x.DeletedAt == null &&
                  x.ThreadId == thread.Id &&
                  x.Type == ThreadActivityType.Visit,
                  cancellationToken: cancellationToken);
      if (visit == null)
      {
        var visitActivity = new ThreadActivity();
        visitActivity.Type = ThreadActivityType.Visit;
        await InsertThreadActivity(threadActivity: visitActivity,
                                   thread: thread,
                                   cancellationToken: cancellationToken);
        return visitActivity;
      }
      return visit;
    }
    public async Task<ThreadActivity> Rate(ThreadActivity ratingCondition, Thread thread, CancellationToken cancellationToken)
    {
      var userId = this.workContext.GetCurrentUserId();
      var oldRate = await threadActivityRepository.GetAsync(predicate: x => x.UserId == userId &&
                 x.DeletedAt == null &&
                 x.ReferenceId == ratingCondition.Id &&
                 x.ThreadId == thread.Id &&
                 x.Type == ThreadActivityType.Rate,
                 cancellationToken: cancellationToken);
      if (oldRate != null)
        await DeleteThreadActivity(oldRate, cancellationToken);
      var rate = new ThreadActivity();
      rate.Type = ThreadActivityType.Rate;
      await InsertThreadActivity(threadActivity: rate,
                                 thread: thread,
                                 reference: ratingCondition,
                                 cancellationToken: cancellationToken);
      return rate;
    }
    public async Task<ThreadActivity> AnswerQuestion(ThreadActivity answer,
                                                     ThreadActivity question,
                                                     CancellationToken cancellationToken)
    {
      answer.Type = ThreadActivityType.Answer;
      await InsertThreadActivity(threadActivity: answer,
                                 thread: question.Thread,
                                 reference: question,
                                 cancellationToken: cancellationToken);
      return answer;
    }
    public async Task<ThreadActivity> InsertCommentReply(ThreadActivity reply,
                                                         ThreadActivity comment,
                                                         CancellationToken cancellationToken)
    {
      reply.Type = ThreadActivityType.Comment;
      await InsertThreadActivity(threadActivity: reply,
                                 thread: comment.Thread,
                                 reference: comment,
                                 cancellationToken: cancellationToken);
      return reply;
    }
    public async Task<ThreadActivity> AddQuestion(ThreadActivity question, Thread thread, CancellationToken cancellationToken)
    {
      question.Type = ThreadActivityType.Question;
      await InsertThreadActivity(threadActivity: question,
                                 thread: thread,
                                 cancellationToken: cancellationToken);
      return question;
    }
    public async Task<ThreadActivity> AddComment(ThreadActivity comment, Thread thread, CancellationToken cancellationToken)
    {
      comment.Type = ThreadActivityType.Comment;
      await InsertThreadActivity(threadActivity: comment,
                                 thread: thread,
                                 cancellationToken: cancellationToken);
      return comment;
    }
    public async Task<ThreadActivity> Rating(ThreadActivity rating,
                                             IList<ThreadActivity> conditions,
                                             Thread thread,
                                             CancellationToken cancellationToken)
    {
      rating.Type = ThreadActivityType.Rating;
      await InsertThreadActivity(threadActivity: rating,
                                 thread: thread,
                                 cancellationToken: cancellationToken);
      foreach (var condition in conditions)
      {
        condition.Type = ThreadActivityType.RatingCondition;
        await InsertThreadActivity(threadActivity: condition,
                                   thread: thread,
                                   reference: rating,
                                   cancellationToken: cancellationToken);
      }
      return rating;
    }
    public async Task<ThreadActivity> AddTag(ThreadActivity tag, Thread thread, CancellationToken cancellationToken)
    {
      tag.Type = ThreadActivityType.Tag;
      await InsertThreadActivity(threadActivity: tag,
                                 thread: thread,
                                 cancellationToken: cancellationToken);
      return tag;
    }
    #endregion
  }
}