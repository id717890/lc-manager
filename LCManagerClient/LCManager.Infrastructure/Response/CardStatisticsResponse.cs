using System;
using Microsoft.Build.Framework;

namespace LCManager.Infrastructure.Response
{
    public class CardStatisticsResponse: BaseResponse
    {
            /// <summary>
            /// уровень карты
            /// </summary>
            public string Level { get; set; }
            /// <summary>
            /// состояние карты
            /// </summary>
            public string Condition { get; set; }
            /// <summary>
            /// баланс карты
            /// </summary>
            public decimal Balance { get; set; }
            /// <summary>
            /// полный баланс?
            /// </summary>
            public decimal FullBalance { get; set; }
            /// <summary>
            /// количество покупок
            /// </summary>
            public int Purchases { get; set; }
            /// <summary>
            /// сумма покупок
            /// </summary>
            public decimal Purchasesum { get; set; }
            /// <summary>
            /// количество возвратов
            /// </summary>
            public int Refunds { get; set; }
            /// <summary>
            /// сумма возвратов
            /// </summary>
            public decimal RefundSum { get; set; }
            /// <summary>
            /// всего потрачено
            /// </summary>
            public decimal SpentSum { get; set; }
            /// <summary>
            /// начисленно балов
            /// </summary>
            public decimal Charged { get; set; }
            /// <summary>
            /// списано баллов
            /// </summary>
            public decimal Redeemed { get; set; }
            /// <summary>
            /// начисленно за возвраты?
            /// </summary>
            public decimal ChargeRefund { get; set; }
            /// <summary>
            /// списано балов за возрат?
            /// </summary>
            public decimal RedeemRefund { get; set; }
            /// <summary>
            /// итоговая скидка
            /// </summary>
            public decimal FullDiscount { get; set; }
            /// <summary>
            /// ID оператора
            /// </summary>
            [Required]
            public Int16 Operator { get; set; }
    }
}
