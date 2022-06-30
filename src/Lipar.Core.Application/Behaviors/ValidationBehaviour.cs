//using FluentValidation;
//using Lipar.Core.Application.Common;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Lipar.Core.Application.Behaviors
//
//    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
//        where TRequest : IRequest<TResponse>
//    {
    //        private readonly IEnumerable<IValidator<TRequest>> validators;
    
    //        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    //        {
        //            this.validators = validators;
    //        }
    
    //        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    //        {
        //            if (validators.Any())
        //            {
            //                var context = new ValidationContext<TRequest>(request);
            //                var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            //                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f is not null).ToList();
            
            //                if (failures.Count != 0)
            //                    throw new ValidationException(failures);
        //            }
        //            return await next();
    //        }
//    }

//    public class ValidationBehavior<TRequest> : IPipelineBehavior<TRequest>
//       where TRequest : IRequest
//    {
    //        private readonly IEnumerable<IValidator<TRequest>> validators;
    
    //        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    //        {
        //            this.validators = validators;
    //        }
    
    //        public async Task Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate next)
    //        {
        //            if (validators.Any())
        //            {
            //                var context = new ValidationContext<TRequest>(request);
            //                var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            //                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f is not null).ToList();
            
            //                if (failures.Count != 0)
            //                    throw new ValidationException(failures);
        //            }
        //            await next();
    //        }
//    }
//

