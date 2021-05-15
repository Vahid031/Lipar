using FluentValidation;
using Lipar.Core.Application.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lipar.Core.Application.Behaviors
{
    public interface IRequestDispacher
    {
        Task<ApplicationResult> Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest;
    }
    public class RequestDispacher : IRequestDispacher
    {
        private readonly IMediator mediator;

        public RequestDispacher(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<ApplicationResult> Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
        {
            ApplicationResult result = new ApplicationResult();
            try
            {
                await mediator.Send(request, cancellationToken);
                result.Status = ApplicationStatus.Done;
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                    result.AddMessage(error.ErrorMessage);
                result.Status = ApplicationStatus.ValidationError;
            }
            catch (Exception ex)
            {
                result.AddMessage(ex.Message);
                result.Status = ApplicationStatus.Exception;
            }
            return await Task.FromResult(result);
        }
    }

    public enum ApplicationStatus
    {
        Done,
        NotFound,
        ValidationError,
        InvalidDomainState,
        Exception
    }

    public class ApplicationResult<T>
    {
        public ApplicationStatus Status { get; set; }
        public List<string> Massage { get; set; }
        public T Data { get; set; }

    }
    public class ApplicationResult
    {
        protected readonly List<string> _messages = new List<string>();
        public IEnumerable<string> Messages => _messages;
        public ApplicationStatus Status { get; set; }

        public void AddMessage(string error) => _messages.Add(error);
        public void ClearMessages() => _messages.Clear();
    }

}
