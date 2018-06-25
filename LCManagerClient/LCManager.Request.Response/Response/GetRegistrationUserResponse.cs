namespace Site.Infrastrucure.Response
{
    using System;

    public class GetRegistrationUserResponse: BaseResponse
    {
        /// <summary>
        /// номер телефона
        /// </summary>
        public Int64 Phone { get; set; }
        /// <summary>
        /// номер карты
        /// </summary>
        public Int64 Card { get; set; }
        /// <summary>
        /// ID участника
        /// </summary>
        public int Client { get; set; }
        /// <summary>
        /// ID торговой точки
        /// </summary>
        public Int16 Pos { get; set; }
    }
}
