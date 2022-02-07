using FluentValidation;
using System;

namespace HeyUrl.IApplication.Base
{
    public class ValidatorFactory : IValidatorFactory
    {
        readonly IServiceProvider _provider;

        public ValidatorFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IValidator<T> GetValidator<T>()
        {
            return (IValidator<T>)_provider.GetService(typeof(IValidator<T>));
        }

        public IValidator GetValidator(Type type)
        {
            return (IValidator)_provider.GetService(typeof(IValidator<>).MakeGenericType(type));
        }
    }
}
