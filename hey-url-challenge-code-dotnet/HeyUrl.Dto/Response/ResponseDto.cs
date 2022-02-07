using System;
using System.Collections.Generic;
using System.Linq;

namespace HeyUrl.Dto.Response
{
    public class ResponseDto
    {
        public IEnumerable<ApplicationMessageDto> Messages { get; set; } = new List<ApplicationMessageDto>();
        public bool IsValid => Messages.All(x => x.Type != ApplicationMessageType.Error);

        public ResponseDto()
        { }

        public ResponseDto(string message, string code = null) : this()
            => AddSuccessResult(message, code);

        public ResponseDto(IEnumerable<ApplicationMessageDto> messages)
            => Messages = messages ?? throw new ArgumentNullException(nameof(messages));

        public void AddSuccessResult(string message, string code = null)
            => AddResult(ApplicationMessageType.Success, message, code);

        public void AddErrorResult(string message, string code = null)
            => AddResult(ApplicationMessageType.Error, message, code);

        private void AddResult(ApplicationMessageType type, string message, string code = null)
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentNullException(nameof(message));

            var messages = Messages.ToList();
            messages.Add(new ApplicationMessageDto { Type = type, Message = message, Code = code });
            Messages = messages;
        }
    }

    public class ResponseDto<T> : ResponseDto
    {
        public T Data { get; set; }

        public ResponseDto() : base()
        { }

        public ResponseDto(T data)
            => Data = data;

        public ResponseDto(T data, string message, string code = null) : base(message, code)
        => Data = data;

        public ResponseDto(T data, IEnumerable<ApplicationMessageDto> messages) : base(messages)
        => Data = data;

        public void UpdateData(T data)
         => Data = data;
    }
}
