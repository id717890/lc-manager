namespace LCManagerPartner.Implementation.Abstractions
{
    /// <summary>
    /// Абстрактный класс для ответа сервера
    /// </summary>
    public abstract class BaseResponse
    {
        /// <summary>
        /// Код ошибки
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
    }
}