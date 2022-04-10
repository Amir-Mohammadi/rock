using System.Collections.Generic;
using rock.Framework.Autofac;
using rock.Core.Domains.Threads;
using rock.Core.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Thread = rock.Core.Domains.Threads.Thread;
namespace rock.Core.Services.Threads
{
  public interface IThreadService : IScopedDependency
  {
    #region Thread
    Task<Thread> newThread(CancellationToken cancellationToken);
    Task DeleteThread(Thread thread, CancellationToken cancellationToken);
    Task<Thread> GetThreadById(int id, CancellationToken cancellationToken, IInclude<Thread> include = null);
    #endregion
    #region ThreadActivity
    Task<ThreadActivity> InsertThreadActivity(ThreadActivity threadActivity, Thread thread, CancellationToken cancellationToken, ThreadActivity reference = null);
    Task PublishThreadActivity(ThreadActivity threadActivity, CancellationToken cancellationToken);
    Task UpdateThreadActivity(ThreadActivity threadActivity, CancellationToken cancellationToken, bool checkOwner = false);
    Task DeleteThreadActivity(ThreadActivity threadActivity, CancellationToken cancellationToken, bool checkOwner = false);
    Task DeleteThreadActiviesByThreadId(int threadId, CancellationToken cancellationToken);
    Task<ThreadActivity> GetThreadActivityById(int id, CancellationToken cancellationToken, int? threadId = null, bool showUnPublished = true, IInclude<ThreadActivity> include = null);
    IQueryable<ThreadActivity> GetThreadActivities(bool showUnPublished = true, IInclude<ThreadActivity> include = null);
    IQueryable<ThreadActivity> GetUserThreadActivities(bool showUnPublished = true, IInclude<ThreadActivity> include = null);
    Task<ThreadActivity> Like(Thread thread, CancellationToken cancellationToken, ThreadActivity activity = null);
    Task Unlike(Thread thread, CancellationToken cancellationToken, ThreadActivity activity = null);
    Task<ThreadActivity> Visit(Thread thread, CancellationToken cancellationToken);
    Task<ThreadActivity> Rate(ThreadActivity ratingCondition, Thread thread, CancellationToken cancellationToken);
    Task<ThreadActivity> AnswerQuestion(ThreadActivity answer, ThreadActivity question, CancellationToken cancellationToken);
    Task<ThreadActivity> InsertCommentReply(ThreadActivity reply, ThreadActivity comment, CancellationToken cancellationToken);
    Task<ThreadActivity> AddQuestion(ThreadActivity question, Thread thread, CancellationToken cancellationToken);
    Task<ThreadActivity> AddComment(ThreadActivity comment, Thread thread, CancellationToken cancellationToken);
    Task<ThreadActivity> Rating(ThreadActivity rating, IList<ThreadActivity> conditions, Thread thread, CancellationToken cancellationToken);
    Task<ThreadActivity> AddTag(ThreadActivity tag, Thread thread, CancellationToken cancellationToken);
    #endregion
  }
}