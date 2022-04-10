using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rock.Core.Common;
using rock.Core.Data;
using rock.Core.Domains.Products;
using rock.Core.Domains.Threads;
using rock.Core.Services.Catalogs;
using rock.Core.Services.Products;
using rock.Core.Services.Threads;
using rock.Factories;
using rock.Filters;
using rock.Models.MarketApi;
using rock.Models.ProductApi;
using rock.OAuth;
namespace rock.Controllers
{
  [Route("/api/v1")]
  [ApiController]
  [Authorize()]
  public class MarketCustomerApiController : BaseController
  {
    #region Fields
    private readonly IMarketStuffFactory marketStuffFactory;
    private readonly IProductService productService;
    private readonly IThreadService threadService;
    private readonly IInclude<Product> productThreadInclude;
    #endregion
    #region Constractor
    public MarketCustomerApiController(IMarketStuffFactory marketStuffFactory,
                                       IProductService productService,
                                       IThreadService threadService)
    {
      this.marketStuffFactory = marketStuffFactory;
      this.productService = productService;
      this.threadService = threadService;
      productThreadInclude = new Include<Product>(query => { return query.Include(x => x.Thread); });
    }
    #endregion
    #region MarketStuffRate
    [HttpPost("market-stuffs/{marketStuffId}/ratings/{ratingConditionId}/rate")]
    public async Task RateOnProductRating([FromRoute] int marketStuffId, [FromRoute] int ratingConditionId, CancellationToken cancellationToken)
    {
      var product = await productService.GetProductById(id: marketStuffId,
                                                        include: productThreadInclude,
                                                        cancellationToken: cancellationToken);
      var ratingCondition = await threadService.GetThreadActivityById(id: ratingConditionId,
                                                                      cancellationToken: cancellationToken);
      await productService.RateProduct(ratingCondition: ratingCondition,
                                       product: product,
                                       cancellationToken: cancellationToken);
    }
    #endregion
    #region MarketStuffQuestion
    [HttpPost("market-stuffs/{marketStuffId}/questions/ask")]
    public async Task AskProductQuestion([FromRoute] int marketStuffId, [FromBody] AskProductQuestionModel newQuestion, CancellationToken cancellationToken)
    {
      var product = await productService.GetProductById(id: marketStuffId,
                                                        include: productThreadInclude,
                                                        cancellationToken: cancellationToken);
      var question = new ThreadActivity
      {
        Payload = newQuestion.Question
      };
      await productService.AskProductQuestion(question: question,
                                              product: product,
                                              cancellationToken: cancellationToken);
    }
    [HttpDelete("market-stuffs/{marketStuffId}/questions/{questionId}")]
    public async Task DeleteMyProductQuestion([FromRoute] int marketStuffId, [FromRoute] int questionId, CancellationToken cancellationToken)
    {
      var question = await productService.GetProductQuestionById(productId: marketStuffId,
                                                                 questionId: questionId,
                                                                 cancellationToken: cancellationToken);
      await productService.DeleteProductQuestion(question: question,
                                                 cancellationToken: cancellationToken,
                                                 checkOwner: true);
    }
    [HttpPut("market-stuffs/{marketStuffId}/questions/{questionId}")]
    public async Task UpdateMyProductQuestion([FromRoute] int marketStuffId, [FromRoute] int questionId, [FromBody] AskProductQuestionModel model, CancellationToken cancellationToken)
    {
      var question = await productService.GetProductQuestionById(productId: marketStuffId,
                                                                 questionId: questionId,
                                                                 cancellationToken: cancellationToken);
      question.Payload = model.Question;
      question.RowVersion = model.RowVersion;
      await productService.UpdateProductQuestion(question: question,
                                                 cancellationToken: cancellationToken,
                                                 checkOwner: true);
    }
    [AllowAnonymous]
    [HttpGet("market-stuffs/{marketStuffId}/questions")]
    public async Task<IPagedList<MarketStuffQuestionModel>> GetProductQuestions([FromRoute] int marketStuffId, [FromQuery] PagedListFilter pagedListFilter, CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketStuffQuestionListModel(marketStuffId: marketStuffId,
                                                                          pagedListFilter: pagedListFilter,
                                                                          cancellationToken: cancellationToken);
    }
    [AllowAnonymous]
    [HttpGet("market-stuffs/{marketStuffId}/questions/{questionId}")]
    public async Task<MarketStuffQuestionModel> GetProductQuestion([FromRoute] int marketStuffId, [FromRoute] int questionId, CancellationToken cancellationToken)
    {
      var result = await marketStuffFactory.PrepareMarketStuffQuestionModel(marketStuffId: marketStuffId,
                                                                            questionId: questionId,
                                                                            cancellationToken: cancellationToken);
      return result;
    }
    #endregion
    #region MarketStuffQuestionAnswer
    [HttpPost("market-stuffs/{marketStuffId}/questions/{questionId}/answer")]
    public async Task AnswerProductQuestion([FromRoute] int marketStuffId, [FromRoute] int questionId, [FromBody] AnswerProductQuestionModel newAnswer, CancellationToken cancellationToken)
    {
      var question = await productService.GetProductQuestionById(productId: marketStuffId,
                                                                 questionId: questionId,
                                                                 cancellationToken: cancellationToken,
                                                                 include: new Include<ThreadActivity>(query =>
                                                                 {
                                                                   query = query.Include(x => x.Thread);
                                                                   return query;
                                                                 }));
      var answer = new ThreadActivity();
      answer.Payload = newAnswer.Answer;
      await productService.AnswerProductQuestion(answer: answer,
                                                 question: question,
                                                 cancellationToken: cancellationToken);
    }
    [HttpDelete("market-stuffs/{marketStuffId}/questions/{questionId}/answers/{answerId}")]
    public async Task DeleteMyProductQuestionAnswer([FromRoute] int marketStuffId, [FromRoute] int questionId, [FromBody] int answerId, CancellationToken cancellationToken)
    {
      var answer = await productService.GetProductAnswer(productId: marketStuffId,
                                                         questionId: questionId,
                                                         answerId: answerId,
                                                         cancellationToken: cancellationToken);
      await productService.DeleteProductAnswer(answer: answer,
                                               cancellationToken: cancellationToken,
                                               checkOwner: true);
    }
    [AllowAnonymous]
    [HttpGet("market-stuffs/{marketStuffId}/questions/{questionId}/answers")]
    public async Task<IPagedList<MarketStuffAnswerModel>> GetPagedProductQuestionAnswers([FromRoute] int marketStuffId, [FromRoute] int questionId, [FromQuery] ProductAnswerSearchParameters parameters, CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketStuffAnswerPagedListModel(marketStuffId, questionId, parameters, cancellationToken);
    }
    [AllowAnonymous]
    [HttpGet("market-stuffs/{marketStuffId}/questions/{questionId}/answers/{answerId}")]
    public async Task<MarketStuffAnswerModel> GetPagedProductQuestionAnswer([FromRoute] int marketStuffId, [FromRoute] int questionId, [FromRoute] int answerId, CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketStuffAnswerModel(marketStuffId: marketStuffId,
                                                            questionId: questionId,
                                                            answerId: answerId,
                                                            cancellationToken: cancellationToken);
    }
    #endregion
    #region MarketStuffComments
    [HttpPost("market-stuffs/{marketStuffId}/comments/write")]
    public async Task WriteProductComment([FromRoute] int marketStuffId, [FromBody] WriteCommentModel newComment, CancellationToken cancellationToken)
    {
      var product = await productService.GetProductById(id: marketStuffId,
                                                        cancellationToken: cancellationToken,
                                                        include: productThreadInclude);
      var comment = new ThreadActivity();
      comment.Payload = newComment.Text;
      await productService.InsertProductComment(comment: comment, product: product, cancellationToken);
    }
    [HttpPatch("market-stuffs/{marketStuffId}/comments/{commentId}")]
    public async Task EditMyProductComment([FromRoute] int marketStuffId, [FromRoute] int commentId, [FromBody] WriteCommentModel updatedComment, CancellationToken cancellationToken)
    {
      var comment = await productService.GetProductCommentById(productId: marketStuffId,
                                                               commentId: commentId,
                                                               cancellationToken: cancellationToken);
      comment.Payload = updatedComment.Text;
      await productService.UpdateProductComment(comment: comment,
                                                cancellationToken: cancellationToken,
                                                checkOwner: true);
    }
    [HttpDelete("market-stuffs/{marketStuffId}/comments/{commentId}")]
    public async Task DeleteMyProductComment([FromRoute] int marketStuffId, [FromRoute] int commentId, CancellationToken cancellationToken)
    {
      var comment = await productService.GetProductCommentById(productId: marketStuffId,
                                                               commentId: commentId,
                                                               cancellationToken: cancellationToken);
      await productService.DeleteProductComment(comment: comment,
                                                cancellationToken: cancellationToken,
                                                checkOwner: true);
    }
    [AllowAnonymous]
    [HttpGet("market-stuffs/{marketStuffId}/comments/{commentId}")]
    public async Task<MarketStuffCommentModel> GetProductComment([FromRoute] int marketStuffId, [FromRoute] int commentId, CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketStuffCommentModel(marketStuffId, commentId, cancellationToken);
    }
    [AllowAnonymous]
    [HttpGet("market-stuffs/{marketStuffId}/comments")]
    public async Task<IPagedList<MarketStuffCommentModel>> GetPagedProductComments([FromRoute] int marketStuffId, [FromQuery] ProductCommentSearchParameters parameters, CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketStuffCommentPagedListModel(marketStuffId, parameters, cancellationToken);
    }
    #endregion
    #region MarketStuffCommentsLike
    [HttpPost("market-stuffs/{marketStuffId}/comments/{commentId}/like")]
    public async Task LikeProductComment([FromRoute] int marketStuffId, [FromRoute] int commentId, CancellationToken cancellationToken)
    {
      var comment = await productService.GetProductCommentById(productId: marketStuffId,
                                                               commentId: commentId,
                                                               cancellationToken: cancellationToken,
                                                               include: new Include<ThreadActivity>(query =>
                                                               {
                                                                 query = query.Include(x => x.Thread);
                                                                 return query;
                                                               }));
      await productService.LikeProductComment(comment: comment,
                                              cancellationToken);
    }
    [HttpPost("market-stuffs/{marketStuffId}/comments/{commentId}/unlike")]
    public async Task DislikeProductComment([FromRoute] int marketStuffId, [FromRoute] int commentId, CancellationToken cancellationToken)
    {
      var comment = await productService.GetProductCommentById(productId: marketStuffId,
                                                               commentId: commentId,
                                                               cancellationToken: cancellationToken,
                                                               include: new Include<ThreadActivity>(query =>
                                                               {
                                                                 query = query.Include(x => x.Thread);
                                                                 return query;
                                                               }));
      await productService.UnlikeProductComment(comment: comment, cancellationToken: cancellationToken);
    }
    #endregion
    #region MarketStuffCommentReply
    [HttpPost("market-stuffs/{marketStuffId}/comments/{commentId}/reply")]
    public async Task CreateProductCommentReply([FromRoute] int marketStuffId, [FromRoute] int commentId, [FromBody] WriteCommentModel newReply, CancellationToken cancellationToken)
    {
      var comment = await productService.GetProductCommentById(productId: marketStuffId,
                                                               commentId: commentId,
                                                               cancellationToken: cancellationToken,
                                                               include: new Include<ThreadActivity>(query =>
                                                               {
                                                                 query = query.Include(x => x.Thread);
                                                                 return query;
                                                               }));
      var reply = new ThreadActivity();
      reply.Payload = newReply.Text;
      await productService.InsertProductCommentReply(reply: reply,
                                                     comment: comment,
                                                     cancellationToken: cancellationToken);
    }
    [HttpPatch("market-stuffs/{marketStuffId}/comments/{commentId}/replies/{replyId}")]
    public async Task EditMyProductCommentReply([FromRoute] int marketStuffId, [FromRoute] int commentId, [FromRoute] int replyId, [FromBody] WriteCommentModel updatedReply, CancellationToken cancellationToken)
    {
      var reply = await productService.GetProductCommentReplyById(productId: marketStuffId,
                                                                  commentId: commentId,
                                                                  replyId: replyId,
                                                                  cancellationToken: cancellationToken);
      reply.Payload = updatedReply.Text;
      await productService.UpdateProductCommentReply(reply: reply,
                                                     cancellationToken: cancellationToken,
                                                     checkOwner: true);
    }
    [HttpDelete("market-stuffs/{marketStuffId}/comments/{commentId}/replies/{replyId}")]
    public async Task DeleteMyProductCommentReply([FromRoute] int marketStuffId, [FromRoute] int commentId, [FromRoute] int replyId, CancellationToken cancellationToken)
    {
      var reply = await productService.GetProductCommentReplyById(productId: marketStuffId,
                                                                  commentId: commentId,
                                                                  replyId: replyId,
                                                                  cancellationToken: cancellationToken);
      await productService.DeleteProductCommentReply(reply: reply,
                                                     cancellationToken: cancellationToken,
                                                     checkOwner: true);
    }
    [AllowAnonymous]
    [HttpGet("market-stuffs/{marketStuffId}/comments/{commentId}/replies/{replyId}")]
    public async Task<MarketStuffCommentReplyModel> GetProductCommentReply([FromRoute] int marketStuffId, [FromRoute] int commentId, [FromRoute] int replyId, CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketStuffCommentReplyModel(marketStuffId: marketStuffId,
                                                                  commentId: commentId,
                                                                  replyId: replyId,
                                                                  cancellationToken: cancellationToken);
    }
    [AllowAnonymous]
    [HttpGet("market-stuffs/{marketStuffId}/comments/{commentId}/replies")]
    public async Task<IPagedList<MarketStuffCommentReplyModel>> GetPagedProductCommentReplies([FromRoute] int marketStuffId, [FromRoute] int commentId, [FromQuery] ProductCommentReplySearchParameters parameters, CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketStuffCommentReplyPagedListModel(marketStuffId: marketStuffId,
                                                                           commentId: commentId,
                                                                           parameters: parameters,
                                                                           cancellationToken: cancellationToken);
    }
    #endregion
    #region MarketStuffLike
    [HttpPost("market-stuffs/{marketStuffId}/like")]
    public async Task LikeProduct([FromRoute] int marketStuffId, CancellationToken cancellationToken)
    {
      var product = await productService.GetProductById(id: marketStuffId,
                                                        cancellationToken: cancellationToken,
                                                        include: productThreadInclude);
      await productService.LikeProduct(product: product,
                                       cancellationToken: cancellationToken);
    }
    [HttpDelete("market-stuffs/{marketStuffId}/unlike")]
    public async Task DislikeProduct([FromRoute] int marketStuffId, CancellationToken cancellationToken)
    {
      var product = await productService.GetProductById(id: marketStuffId,
                                                        cancellationToken: cancellationToken,
                                                        include: productThreadInclude);
      await productService.UnlikeProduct(product: product,
                                         cancellationToken: cancellationToken);
    }
    #endregion
  }
}