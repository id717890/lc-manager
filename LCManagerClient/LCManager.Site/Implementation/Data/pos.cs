namespace LC_Manager.Implementation.Data
{
    using System;

    public class Pos
    {
        /// <summary>
        /// Код торговой точки
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// наименование ТТ
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// регион
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// город
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// адрес ТТ
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// номер телефона
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Показывать на сайте?
        /// </summary>
        public Boolean ShowOnSite { get; set; }
        /// <summary>
        /// Выдает карты участника?
        /// </summary>
        public Boolean GivesCard { get; set; }

        public string Check { get; set; }

        public Pos() { }
        public Pos(int id, string code, string region, string city, string address, string phone, bool showonsite, bool givescard)
        {
            Id = id;
            Name = code;
            Region = region;
            City = city;
            Address = address;
            Phone = phone;
            ShowOnSite = showonsite;
            GivesCard = givescard;
        }
    }
}