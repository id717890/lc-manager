namespace Site.Infrastrucure
{
    public static class PhoneService
    {
        public static long GetPhoneFromStr(string phone)
        {
            if (long.TryParse(phone.Replace("+7", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ",""), out var number))
            {
                return number;
            }
            return 0;
        }
    }
}
