﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace PlizcardPos
{
    /// <remarks/>
    [System.Web.Services.WebServiceAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "POSProcessing", Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public partial class POSProcessing : System.Web.Services.WebService
    {

        /// <remarks/>
        [System.Web.Services.WebMethod()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://loyalty.manzanagroup.ru/loyalty.xsd/ProcessRequest", 
            RequestNamespace = "http://loyalty.manzanagroup.ru/loyalty.xsd", 
            ResponseNamespace = "http://loyalty.manzanagroup.ru/loyalty.xsd", 
            Use = System.Web.Services.Description.SoapBindingUse.Literal, 
            ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        //[System.Xml.Serialization.XmlIncludeAttribute(typeof(ContactInfoUpdateResponseBase))]
        //[System.Xml.Serialization.XmlIncludeAttribute(typeof(CardResponseBase))]
        //[System.Xml.Serialization.XmlIncludeAttribute(typeof(OfferResponseBase))]
        //[System.Xml.Serialization.XmlIncludeAttribute(typeof(BalanceResponseBase))]
        //[System.Xml.Serialization.XmlIncludeAttribute(typeof(ChequeResponseBase))]
        //[System.Xml.Serialization.SoapInclude(typeof(ContactInfoUpdateResponseBase))]
        //[System.Xml.Serialization.SoapInclude(typeof(CardResponseBase))]
        //[System.Xml.Serialization.SoapInclude(typeof(OfferResponseBase))]
        //[System.Xml.Serialization.SoapInclude(typeof(BalanceResponseBase))]
        [System.Xml.Serialization.SoapInclude(typeof(ChequeResponseBase))]
        public ProcessRequestResponseProcessRequestResult ProcessRequest(ProcessRequestRequest request, string orgName)
        {            
            var response = new ProcessRequestResponseProcessRequestResult();
            switch(request.ItemsElementName[0])
            {
                case ItemsChoiceType.ChequeRequest:
                    var cheque = (Cheque)request.Items[0];
                    var item = new ChequeResponseBase();
                    ChequeResponseBase[] items = new ChequeResponseBase[1];
                    if (cheque.ChequeType == ChequeType.Soft)
                    {                        
                        items[0] = new ChequeResponseBase
                        {
                            TransactionID = "-9223372036854726434",
                            RequestID = "1002",
                            Processed = Convert.ToDateTime("2017-09-06T04:23:43.79"),
                            ReturnCode = "0",
                            Message = "OK",
                            CardBalance = 0.00M,
                            CardActiveBalance = 0.00M,
                            CardSumm = 1000.00M,
                            CardDiscount = 0.000M,
                            Summ = 1000.00M,
                            Discount = 0.000M,
                            SummDiscounted = 1000.00M,
                            ChargedBonus = 0.00M,
                            ActiveChargedBonus = 0.00M,
                            AvailablePayment = 0.00M,
                            WriteoffBonus = 0.00M
                        };
                    }                    
                    
                    response.Items = items;
                    break;
                case ItemsChoiceType.BalanceRequest:
                    break;
                case ItemsChoiceType.BonusRequest:
                    break;
                case ItemsChoiceType.CardRequest:
                    break;
                case ItemsChoiceType.ContactInfoUpdateRequest:
                    break;
                case ItemsChoiceType.OfferRequest:
                    break;
            }

            
            //var item = new ResponseBase { Message = "Hello", ReturnCode = "Code"};
            
            return response;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public partial class ProcessRequestRequest
    {

        private RequestBase[] itemsField;

        private ItemsChoiceType[] itemsElementNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("BalanceRequest", typeof(RequestBase))]
        [System.Xml.Serialization.XmlElementAttribute("BonusRequest", typeof(BonusRequestBase))]
        [System.Xml.Serialization.XmlElementAttribute("CardRequest", typeof(CardRequestBase))]
        [System.Xml.Serialization.XmlElementAttribute("ChequeRequest", typeof(Cheque))]
        [System.Xml.Serialization.XmlElementAttribute("ContactInfoUpdateRequest", typeof(ContactInfoUpdateRequestBase))]
        [System.Xml.Serialization.XmlElementAttribute("OfferRequest", typeof(Cheque))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public RequestBase[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BonusRequestBase))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContactInfoUpdateRequestBase))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CardRequestBase))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Cheque))]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public partial class RequestBase
    {

        private int timeoutField;

        private bool timeoutFieldSpecified;

        private string requestIDField;

        private Card cardField;

        private MobilePhone mobilePhoneField;

        private System.DateTime dateTimeField;

        private string organizationField;

        private string businessUnitField;

        private string pOSField;

        private WriteOffConfirm writeOffConfirmField;

        private ChequeReference chequeReferenceField;

        private TransactionReference transactionReferenceField;

        /// <remarks/>
        public int Timeout
        {
            get
            {
                return this.timeoutField;
            }
            set
            {
                this.timeoutField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TimeoutSpecified
        {
            get
            {
                return this.timeoutFieldSpecified;
            }
            set
            {
                this.timeoutFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string RequestID
        {
            get
            {
                return this.requestIDField;
            }
            set
            {
                this.requestIDField = value;
            }
        }

        /// <remarks/>
        public Card Card
        {
            get
            {
                return this.cardField;
            }
            set
            {
                this.cardField = value;
            }
        }

        /// <remarks/>
        public MobilePhone MobilePhone
        {
            get
            {
                return this.mobilePhoneField;
            }
            set
            {
                this.mobilePhoneField = value;
            }
        }

        /// <remarks/>
        public System.DateTime DateTime
        {
            get
            {
                return this.dateTimeField;
            }
            set
            {
                this.dateTimeField = value;
            }
        }

        /// <remarks/>
        public string Organization
        {
            get
            {
                return this.organizationField;
            }
            set
            {
                this.organizationField = value;
            }
        }

        /// <remarks/>
        public string BusinessUnit
        {
            get
            {
                return this.businessUnitField;
            }
            set
            {
                this.businessUnitField = value;
            }
        }

        /// <remarks/>
        public string POS
        {
            get
            {
                return this.pOSField;
            }
            set
            {
                this.pOSField = value;
            }
        }

        /// <remarks/>
        public WriteOffConfirm WriteOffConfirm
        {
            get
            {
                return this.writeOffConfirmField;
            }
            set
            {
                this.writeOffConfirmField = value;
            }
        }

        /// <remarks/>
        public ChequeReference ChequeReference
        {
            get
            {
                return this.chequeReferenceField;
            }
            set
            {
                this.chequeReferenceField = value;
            }
        }

        /// <remarks/>
        public TransactionReference TransactionReference
        {
            get
            {
                return this.transactionReferenceField;
            }
            set
            {
                this.transactionReferenceField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public partial class Card
    {

        private string itemField;

        private ItemChoiceType itemElementNameField;

        private BonusType_Type bonusTypeField;

        private bool bonusTypeFieldSpecified;

        private decimal discountField;

        private bool discountFieldSpecified;

        private Status_Type statusField;

        private bool statusFieldSpecified;

        private CollaborationType_Type collaborationTypeField;

        private bool collaborationTypeFieldSpecified;

        private string cardTypeIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CardNumber", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("Track2", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("Tracks", typeof(string))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public string Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
            }
        }

        /// <remarks/>
        public BonusType_Type BonusType
        {
            get
            {
                return this.bonusTypeField;
            }
            set
            {
                this.bonusTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BonusTypeSpecified
        {
            get
            {
                return this.bonusTypeFieldSpecified;
            }
            set
            {
                this.bonusTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal Discount
        {
            get
            {
                return this.discountField;
            }
            set
            {
                this.discountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DiscountSpecified
        {
            get
            {
                return this.discountFieldSpecified;
            }
            set
            {
                this.discountFieldSpecified = value;
            }
        }

        /// <remarks/>
        public Status_Type Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StatusSpecified
        {
            get
            {
                return this.statusFieldSpecified;
            }
            set
            {
                this.statusFieldSpecified = value;
            }
        }

        /// <remarks/>
        public CollaborationType_Type CollaborationType
        {
            get
            {
                return this.collaborationTypeField;
            }
            set
            {
                this.collaborationTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CollaborationTypeSpecified
        {
            get
            {
                return this.collaborationTypeFieldSpecified;
            }
            set
            {
                this.collaborationTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string CardTypeID
        {
            get
            {
                return this.cardTypeIDField;
            }
            set
            {
                this.cardTypeIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd", IncludeInSchema = false)]
    public enum ItemChoiceType
    {

        /// <remarks/>
        CardNumber,

        /// <remarks/>
        Track2,

        /// <remarks/>
        Tracks,
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public enum BonusType_Type
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Item3,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("4")]
        Item4,
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public enum Status_Type
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Item3,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("4")]
        Item4,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5")]
        Item5,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("6")]
        Item6,
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public enum CollaborationType_Type
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Item3,
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public partial class OfferElement
    {

        private string offerElementNameField;

        private string offerElementIDField;

        private string articleField;

        private string articleGroupField;

        private string articleExtGroupField;

        private string manufacturerField;

        private decimal quantityField;

        /// <remarks/>
        public string OfferElementName
        {
            get
            {
                return this.offerElementNameField;
            }
            set
            {
                this.offerElementNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string OfferElementID
        {
            get
            {
                return this.offerElementIDField;
            }
            set
            {
                this.offerElementIDField = value;
            }
        }

        /// <remarks/>
        public string Article
        {
            get
            {
                return this.articleField;
            }
            set
            {
                this.articleField = value;
            }
        }

        /// <remarks/>
        public string ArticleGroup
        {
            get
            {
                return this.articleGroupField;
            }
            set
            {
                this.articleGroupField = value;
            }
        }

        /// <remarks/>
        public string ArticleExtGroup
        {
            get
            {
                return this.articleExtGroupField;
            }
            set
            {
                this.articleExtGroupField = value;
            }
        }

        /// <remarks/>
        public string Manufacturer
        {
            get
            {
                return this.manufacturerField;
            }
            set
            {
                this.manufacturerField = value;
            }
        }

        /// <remarks/>
        public decimal Quantity
        {
            get
            {
                return this.quantityField;
            }
            set
            {
                this.quantityField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public partial class Offer
    {

        private string offerNameField;

        private string offerIDField;

        private OfferElement[] offerElementField;

        /// <remarks/>
        public string OfferName
        {
            get
            {
                return this.offerNameField;
            }
            set
            {
                this.offerNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string OfferID
        {
            get
            {
                return this.offerIDField;
            }
            set
            {
                this.offerIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("OfferElement")]
        public OfferElement[] OfferElement
        {
            get
            {
                return this.offerElementField;
            }
            set
            {
                this.offerElementField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public partial class InstantCoupon
    {

        private string numberField;

        private string typeField;

        private string messageField;

        private System.DateTime effectDateField;

        private bool effectDateFieldSpecified;

        private System.DateTime expiryDateField;

        private bool expiryDateFieldSpecified;

        /// <remarks/>
        public string Number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
            }
        }

        /// <remarks/>
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public string Message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        public System.DateTime EffectDate
        {
            get
            {
                return this.effectDateField;
            }
            set
            {
                this.effectDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EffectDateSpecified
        {
            get
            {
                return this.effectDateFieldSpecified;
            }
            set
            {
                this.effectDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime ExpiryDate
        {
            get
            {
                return this.expiryDateField;
            }
            set
            {
                this.expiryDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExpiryDateSpecified
        {
            get
            {
                return this.expiryDateFieldSpecified;
            }
            set
            {
                this.expiryDateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public partial class PersonalOfferType
    {

        private string textField;

        private string idField;

        private string descriptionField;

        private decimal valueField;

        private bool valueFieldSpecified;

        private decimal quantityGoodsField;

        private bool quantityGoodsFieldSpecified;

        private decimal priceOfQuantityField;

        private bool priceOfQuantityFieldSpecified;

        private decimal minquantityField;

        private bool minquantityFieldSpecified;

        private System.DateTime effectDateField;

        private bool effectDateFieldSpecified;

        private System.DateTime expiryDateField;

        private bool expiryDateFieldSpecified;

        private string subjectIDField;

        private string subjectNameField;

        private string subjectUrlField;

        private string printField;

        private long internalIDField;

        private bool internalIDFieldSpecified;

        /// <remarks/>
        public string Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public decimal Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ValueSpecified
        {
            get
            {
                return this.valueFieldSpecified;
            }
            set
            {
                this.valueFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal QuantityGoods
        {
            get
            {
                return this.quantityGoodsField;
            }
            set
            {
                this.quantityGoodsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool QuantityGoodsSpecified
        {
            get
            {
                return this.quantityGoodsFieldSpecified;
            }
            set
            {
                this.quantityGoodsFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal PriceOfQuantity
        {
            get
            {
                return this.priceOfQuantityField;
            }
            set
            {
                this.priceOfQuantityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PriceOfQuantitySpecified
        {
            get
            {
                return this.priceOfQuantityFieldSpecified;
            }
            set
            {
                this.priceOfQuantityFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal Minquantity
        {
            get
            {
                return this.minquantityField;
            }
            set
            {
                this.minquantityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MinquantitySpecified
        {
            get
            {
                return this.minquantityFieldSpecified;
            }
            set
            {
                this.minquantityFieldSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime EffectDate
        {
            get
            {
                return this.effectDateField;
            }
            set
            {
                this.effectDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EffectDateSpecified
        {
            get
            {
                return this.effectDateFieldSpecified;
            }
            set
            {
                this.effectDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime ExpiryDate
        {
            get
            {
                return this.expiryDateField;
            }
            set
            {
                this.expiryDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExpiryDateSpecified
        {
            get
            {
                return this.expiryDateFieldSpecified;
            }
            set
            {
                this.expiryDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string SubjectID
        {
            get
            {
                return this.subjectIDField;
            }
            set
            {
                this.subjectIDField = value;
            }
        }

        /// <remarks/>
        public string SubjectName
        {
            get
            {
                return this.subjectNameField;
            }
            set
            {
                this.subjectNameField = value;
            }
        }

        /// <remarks/>
        public string SubjectUrl
        {
            get
            {
                return this.subjectUrlField;
            }
            set
            {
                this.subjectUrlField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string Print
        {
            get
            {
                return this.printField;
            }
            set
            {
                this.printField = value;
            }
        }

        /// <remarks/>
        public long InternalID
        {
            get
            {
                return this.internalIDField;
            }
            set
            {
                this.internalIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool InternalIDSpecified
        {
            get
            {
                return this.internalIDFieldSpecified;
            }
            set
            {
                this.internalIDFieldSpecified = value;
            }
        }
    }

    /// <remarks/>        
    [System.Serializable()]
    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    [KnownType(typeof(ChequeResponseBase))]
    public partial class ResponseBase
    {

        private string transactionIDField;

        private string requestIDField;

        private System.DateTime processedField;

        private string returnCodeField;

        private string messageField;

        private PersonalOfferType[] personalOfferField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string TransactionID
        {
            get
            {
                return this.transactionIDField;
            }
            set
            {
                this.transactionIDField = value;
            }
        }

        /// <remarks/>
        public string RequestID
        {
            get
            {
                return this.requestIDField;
            }
            set
            {
                this.requestIDField = value;
            }
        }

        /// <remarks/>
        public System.DateTime Processed
        {
            get
            {
                return this.processedField;
            }
            set
            {
                this.processedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string ReturnCode
        {
            get
            {
                return this.returnCodeField;
            }
            set
            {
                this.returnCodeField = value;
            }
        }

        /// <remarks/>
        public string Message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PersonalOffer")]
        public PersonalOfferType[] PersonalOffer
        {
            get
            {
                return this.personalOfferField;
            }
            set
            {
                this.personalOfferField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public partial class ContactInfoUpdateResponseBase : ResponseBase
    {

        private string cardNumberField;

        /// <remarks/>
        public string CardNumber
        {
            get
            {
                return this.cardNumberField;
            }
            set
            {
                this.cardNumberField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public partial class CardResponseBase : ResponseBase
    {

        private string cashierMessageField;

        private string cashierMessage2Field;

        private string chequeMessageField;

        private string chequeMessage2Field;

        private string personalMessageField;

        private string personalMessage2Field;

        private string firstNameField;

        private string lastNameField;

        private string middleNameField;

        private string fullNameField;

        private System.DateTime birthDateField;

        private bool birthDateFieldSpecified;

        private string ageField;

        private string phoneField;

        private string emailField;

        private Card[] cardField;

        /// <remarks/>
        public string CashierMessage
        {
            get
            {
                return this.cashierMessageField;
            }
            set
            {
                this.cashierMessageField = value;
            }
        }

        /// <remarks/>
        public string CashierMessage2
        {
            get
            {
                return this.cashierMessage2Field;
            }
            set
            {
                this.cashierMessage2Field = value;
            }
        }

        /// <remarks/>
        public string ChequeMessage
        {
            get
            {
                return this.chequeMessageField;
            }
            set
            {
                this.chequeMessageField = value;
            }
        }

        /// <remarks/>
        public string ChequeMessage2
        {
            get
            {
                return this.chequeMessage2Field;
            }
            set
            {
                this.chequeMessage2Field = value;
            }
        }

        /// <remarks/>
        public string PersonalMessage
        {
            get
            {
                return this.personalMessageField;
            }
            set
            {
                this.personalMessageField = value;
            }
        }

        /// <remarks/>
        public string PersonalMessage2
        {
            get
            {
                return this.personalMessage2Field;
            }
            set
            {
                this.personalMessage2Field = value;
            }
        }

        /// <remarks/>
        public string FirstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }

        /// <remarks/>
        public string LastName
        {
            get
            {
                return this.lastNameField;
            }
            set
            {
                this.lastNameField = value;
            }
        }

        /// <remarks/>
        public string MiddleName
        {
            get
            {
                return this.middleNameField;
            }
            set
            {
                this.middleNameField = value;
            }
        }

        /// <remarks/>
        public string FullName
        {
            get
            {
                return this.fullNameField;
            }
            set
            {
                this.fullNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime BirthDate
        {
            get
            {
                return this.birthDateField;
            }
            set
            {
                this.birthDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BirthDateSpecified
        {
            get
            {
                return this.birthDateFieldSpecified;
            }
            set
            {
                this.birthDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string Age
        {
            get
            {
                return this.ageField;
            }
            set
            {
                this.ageField = value;
            }
        }

        /// <remarks/>
        public string Phone
        {
            get
            {
                return this.phoneField;
            }
            set
            {
                this.phoneField = value;
            }
        }

        /// <remarks/>
        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Card")]
        public Card[] Card
        {
            get
            {
                return this.cardField;
            }
            set
            {
                this.cardField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public partial class OfferResponseBase : ResponseBase
    {

        private Offer[] offerField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Offer")]
        public Offer[] Offer
        {
            get
            {
                return this.offerField;
            }
            set
            {
                this.offerField = value;
            }
        }
    }

    /// <remarks/>
    //[System.Xml.Serialization.XmlIncludeAttribute(typeof(ChequeResponseBase))]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    //[KnownType(typeof(ChequeResponseBase))]
    public partial class BalanceResponseBase : ResponseBase
    {

        private Card cardField;

        private string contactIDField;

        private decimal cardBalanceField;

        private bool cardBalanceFieldSpecified;

        private decimal cardNormalBalanceField;

        private bool cardNormalBalanceFieldSpecified;

        private decimal cardStatusBalanceField;

        private bool cardStatusBalanceFieldSpecified;

        private decimal cardActiveBalanceField;

        private bool cardActiveBalanceFieldSpecified;

        private decimal cardNormalActiveBalanceField;

        private bool cardNormalActiveBalanceFieldSpecified;

        private decimal cardStatusActiveBalanceField;

        private bool cardStatusActiveBalanceFieldSpecified;

        private decimal cardSummField;

        private bool cardSummFieldSpecified;

        private decimal cardSummDiscountedField;

        private bool cardSummDiscountedFieldSpecified;

        private decimal cardDiscountField;

        private bool cardDiscountFieldSpecified;

        private string cardQuantityField;

        private ContactPresence_Type contactPresenceField;

        private bool contactPresenceFieldSpecified;

        private string cardTypeField;

        private string cardStatusField;

        private string cardCollaborationTypeField;

        private string cardChargeTypeField;

        private decimal cardChargedBonusField;

        private bool cardChargedBonusFieldSpecified;

        private decimal cardWriteoffBonusField;

        private bool cardWriteoffBonusFieldSpecified;

        private string cashierMessageField;

        private string cashierMessage2Field;

        private string chequeMessageField;

        private string chequeMessage2Field;

        private string personalMessageField;

        private string personalMessage2Field;

        private string firstNameField;

        private string lastNameField;

        private string middleNameField;

        private string fullNameField;

        private System.DateTime birthDateField;

        private bool birthDateFieldSpecified;

        private string ageField;

        private string phoneField;

        private string emailField;

        private string codeWordField;

        /// <remarks/>
        public Card Card
        {
            get
            {
                return this.cardField;
            }
            set
            {
                this.cardField = value;
            }
        }

        /// <remarks/>
        public string ContactID
        {
            get
            {
                return this.contactIDField;
            }
            set
            {
                this.contactIDField = value;
            }
        }

        /// <remarks/>
        public decimal CardBalance
        {
            get
            {
                return this.cardBalanceField;
            }
            set
            {
                this.cardBalanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CardBalanceSpecified
        {
            get
            {
                return this.cardBalanceFieldSpecified;
            }
            set
            {
                this.cardBalanceFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal CardNormalBalance
        {
            get
            {
                return this.cardNormalBalanceField;
            }
            set
            {
                this.cardNormalBalanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CardNormalBalanceSpecified
        {
            get
            {
                return this.cardNormalBalanceFieldSpecified;
            }
            set
            {
                this.cardNormalBalanceFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal CardStatusBalance
        {
            get
            {
                return this.cardStatusBalanceField;
            }
            set
            {
                this.cardStatusBalanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CardStatusBalanceSpecified
        {
            get
            {
                return this.cardStatusBalanceFieldSpecified;
            }
            set
            {
                this.cardStatusBalanceFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal CardActiveBalance
        {
            get
            {
                return this.cardActiveBalanceField;
            }
            set
            {
                this.cardActiveBalanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CardActiveBalanceSpecified
        {
            get
            {
                return this.cardActiveBalanceFieldSpecified;
            }
            set
            {
                this.cardActiveBalanceFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal CardNormalActiveBalance
        {
            get
            {
                return this.cardNormalActiveBalanceField;
            }
            set
            {
                this.cardNormalActiveBalanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CardNormalActiveBalanceSpecified
        {
            get
            {
                return this.cardNormalActiveBalanceFieldSpecified;
            }
            set
            {
                this.cardNormalActiveBalanceFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal CardStatusActiveBalance
        {
            get
            {
                return this.cardStatusActiveBalanceField;
            }
            set
            {
                this.cardStatusActiveBalanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CardStatusActiveBalanceSpecified
        {
            get
            {
                return this.cardStatusActiveBalanceFieldSpecified;
            }
            set
            {
                this.cardStatusActiveBalanceFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal CardSumm
        {
            get
            {
                return this.cardSummField;
            }
            set
            {
                this.cardSummField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CardSummSpecified
        {
            get
            {
                return this.cardSummFieldSpecified;
            }
            set
            {
                this.cardSummFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal CardSummDiscounted
        {
            get
            {
                return this.cardSummDiscountedField;
            }
            set
            {
                this.cardSummDiscountedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CardSummDiscountedSpecified
        {
            get
            {
                return this.cardSummDiscountedFieldSpecified;
            }
            set
            {
                this.cardSummDiscountedFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal CardDiscount
        {
            get
            {
                return this.cardDiscountField;
            }
            set
            {
                this.cardDiscountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CardDiscountSpecified
        {
            get
            {
                return this.cardDiscountFieldSpecified;
            }
            set
            {
                this.cardDiscountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string CardQuantity
        {
            get
            {
                return this.cardQuantityField;
            }
            set
            {
                this.cardQuantityField = value;
            }
        }

        /// <remarks/>
        public ContactPresence_Type ContactPresence
        {
            get
            {
                return this.contactPresenceField;
            }
            set
            {
                this.contactPresenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ContactPresenceSpecified
        {
            get
            {
                return this.contactPresenceFieldSpecified;
            }
            set
            {
                this.contactPresenceFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string CardType
        {
            get
            {
                return this.cardTypeField;
            }
            set
            {
                this.cardTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string CardStatus
        {
            get
            {
                return this.cardStatusField;
            }
            set
            {
                this.cardStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string CardCollaborationType
        {
            get
            {
                return this.cardCollaborationTypeField;
            }
            set
            {
                this.cardCollaborationTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string CardChargeType
        {
            get
            {
                return this.cardChargeTypeField;
            }
            set
            {
                this.cardChargeTypeField = value;
            }
        }

        /// <remarks/>
        public decimal CardChargedBonus
        {
            get
            {
                return this.cardChargedBonusField;
            }
            set
            {
                this.cardChargedBonusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CardChargedBonusSpecified
        {
            get
            {
                return this.cardChargedBonusFieldSpecified;
            }
            set
            {
                this.cardChargedBonusFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal CardWriteoffBonus
        {
            get
            {
                return this.cardWriteoffBonusField;
            }
            set
            {
                this.cardWriteoffBonusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CardWriteoffBonusSpecified
        {
            get
            {
                return this.cardWriteoffBonusFieldSpecified;
            }
            set
            {
                this.cardWriteoffBonusFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string CashierMessage
        {
            get
            {
                return this.cashierMessageField;
            }
            set
            {
                this.cashierMessageField = value;
            }
        }

        /// <remarks/>
        public string CashierMessage2
        {
            get
            {
                return this.cashierMessage2Field;
            }
            set
            {
                this.cashierMessage2Field = value;
            }
        }

        /// <remarks/>
        public string ChequeMessage
        {
            get
            {
                return this.chequeMessageField;
            }
            set
            {
                this.chequeMessageField = value;
            }
        }

        /// <remarks/>
        public string ChequeMessage2
        {
            get
            {
                return this.chequeMessage2Field;
            }
            set
            {
                this.chequeMessage2Field = value;
            }
        }

        /// <remarks/>
        public string PersonalMessage
        {
            get
            {
                return this.personalMessageField;
            }
            set
            {
                this.personalMessageField = value;
            }
        }

        /// <remarks/>
        public string PersonalMessage2
        {
            get
            {
                return this.personalMessage2Field;
            }
            set
            {
                this.personalMessage2Field = value;
            }
        }

        /// <remarks/>
        public string FirstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }

        /// <remarks/>
        public string LastName
        {
            get
            {
                return this.lastNameField;
            }
            set
            {
                this.lastNameField = value;
            }
        }

        /// <remarks/>
        public string MiddleName
        {
            get
            {
                return this.middleNameField;
            }
            set
            {
                this.middleNameField = value;
            }
        }

        /// <remarks/>
        public string FullName
        {
            get
            {
                return this.fullNameField;
            }
            set
            {
                this.fullNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime BirthDate
        {
            get
            {
                return this.birthDateField;
            }
            set
            {
                this.birthDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BirthDateSpecified
        {
            get
            {
                return this.birthDateFieldSpecified;
            }
            set
            {
                this.birthDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string Age
        {
            get
            {
                return this.ageField;
            }
            set
            {
                this.ageField = value;
            }
        }

        /// <remarks/>
        public string Phone
        {
            get
            {
                return this.phoneField;
            }
            set
            {
                this.phoneField = value;
            }
        }

        /// <remarks/>
        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }

        /// <remarks/>
        public string CodeWord
        {
            get
            {
                return this.codeWordField;
            }
            set
            {
                this.codeWordField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public enum ContactPresence_Type
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("0")]
        Item0,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    [System.Xml.Serialization.XmlRoot("ChequeResponseBase")]
    public partial class ChequeResponseBase : BalanceResponseBase
    {
        private decimal summField;

        private bool summFieldSpecified;

        private decimal discountField;

        private bool discountFieldSpecified;

        private decimal summDiscountedField;

        private bool summDiscountedFieldSpecified;

        private decimal chargedBonusField;

        private bool chargedBonusFieldSpecified;

        private decimal activeChargedBonusField;

        private bool activeChargedBonusFieldSpecified;

        private decimal chargedStatusBonusField;

        private bool chargedStatusBonusFieldSpecified;

        private decimal activeChargedStatusBonusField;

        private bool activeChargedStatusBonusFieldSpecified;

        private decimal availablePaymentField;

        private bool availablePaymentFieldSpecified;

        private decimal writeoffBonusField;

        private bool writeoffBonusFieldSpecified;

        private decimal writeoffStatusBonusField;

        private bool writeoffStatusBonusFieldSpecified;

        private Coupon[] couponsField;

        private InstantCoupon[] instantCouponsField;

        private ExtendedAttributes[] extendedAttributeField;

        private ChequeItem[] itemField;

        private Offer[] offerField;

        /// <remarks/>
        public decimal Summ
        {
            get
            {
                return this.summField;
            }
            set
            {
                this.summField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SummSpecified
        {
            get
            {
                return this.summFieldSpecified;
            }
            set
            {
                this.summFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal Discount
        {
            get
            {
                return this.discountField;
            }
            set
            {
                this.discountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DiscountSpecified
        {
            get
            {
                return this.discountFieldSpecified;
            }
            set
            {
                this.discountFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal SummDiscounted
        {
            get
            {
                return this.summDiscountedField;
            }
            set
            {
                this.summDiscountedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SummDiscountedSpecified
        {
            get
            {
                return this.summDiscountedFieldSpecified;
            }
            set
            {
                this.summDiscountedFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal ChargedBonus
        {
            get
            {
                return this.chargedBonusField;
            }
            set
            {
                this.chargedBonusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ChargedBonusSpecified
        {
            get
            {
                return this.chargedBonusFieldSpecified;
            }
            set
            {
                this.chargedBonusFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal ActiveChargedBonus
        {
            get
            {
                return this.activeChargedBonusField;
            }
            set
            {
                this.activeChargedBonusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActiveChargedBonusSpecified
        {
            get
            {
                return this.activeChargedBonusFieldSpecified;
            }
            set
            {
                this.activeChargedBonusFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal ChargedStatusBonus
        {
            get
            {
                return this.chargedStatusBonusField;
            }
            set
            {
                this.chargedStatusBonusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ChargedStatusBonusSpecified
        {
            get
            {
                return this.chargedStatusBonusFieldSpecified;
            }
            set
            {
                this.chargedStatusBonusFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal ActiveChargedStatusBonus
        {
            get
            {
                return this.activeChargedStatusBonusField;
            }
            set
            {
                this.activeChargedStatusBonusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActiveChargedStatusBonusSpecified
        {
            get
            {
                return this.activeChargedStatusBonusFieldSpecified;
            }
            set
            {
                this.activeChargedStatusBonusFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal AvailablePayment
        {
            get
            {
                return this.availablePaymentField;
            }
            set
            {
                this.availablePaymentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AvailablePaymentSpecified
        {
            get
            {
                return this.availablePaymentFieldSpecified;
            }
            set
            {
                this.availablePaymentFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal WriteoffBonus
        {
            get
            {
                return this.writeoffBonusField;
            }
            set
            {
                this.writeoffBonusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool WriteoffBonusSpecified
        {
            get
            {
                return this.writeoffBonusFieldSpecified;
            }
            set
            {
                this.writeoffBonusFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal WriteoffStatusBonus
        {
            get
            {
                return this.writeoffStatusBonusField;
            }
            set
            {
                this.writeoffStatusBonusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool WriteoffStatusBonusSpecified
        {
            get
            {
                return this.writeoffStatusBonusFieldSpecified;
            }
            set
            {
                this.writeoffStatusBonusFieldSpecified = value;
            }
        }

        /// <remarks/>        
        [System.Xml.Serialization.XmlArray("Coupons")]
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public Coupon[] Coupons
        {
            get
            {
                return this.couponsField;
            }
            set
            {
                this.couponsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public InstantCoupon[] InstantCoupons
        {
            get
            {
                return this.instantCouponsField;
            }
            set
            {
                this.instantCouponsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ExtendedAttribute")]
        public ExtendedAttributes[] ExtendedAttribute
        {
            get
            {
                return this.extendedAttributeField;
            }
            set
            {
                this.extendedAttributeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Item")]
        public ChequeItem[] Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Offer")]
        public Offer[] Offer
        {
            get
            {
                return this.offerField;
            }
            set
            {
                this.offerField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute("Coupon", Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]    
    public partial class Coupon
    {

        private string numberField;

        private string emissionIDField;

        private string typeIDField;

        private string applicabilityMessageField;

        private string applicabilityCodeField;

        /// <remarks/>
        public string Number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
            }
        }

        /// <remarks/>
        public string EmissionID
        {
            get
            {
                return this.emissionIDField;
            }
            set
            {
                this.emissionIDField = value;
            }
        }

        /// <remarks/>
        public string TypeID
        {
            get
            {
                return this.typeIDField;
            }
            set
            {
                this.typeIDField = value;
            }
        }

        /// <remarks/>
        public string ApplicabilityMessage
        {
            get
            {
                return this.applicabilityMessageField;
            }
            set
            {
                this.applicabilityMessageField = value;
            }
        }

        /// <remarks/>
        public string ApplicabilityCode
        {
            get
            {
                return this.applicabilityCodeField;
            }
            set
            {
                this.applicabilityCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public partial class ExtendedAttributes
    {

        private string keyField;

        private string valueField;

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public partial class ChequeItem
    {

        private string positionNumberField;

        private string articleField;

        private string articleNameField;

        private decimal priceField;

        private decimal quantityField;

        private decimal summField;

        private decimal discountField;

        private decimal summDiscountedField;

        private decimal availablePaymentField;

        private bool availablePaymentFieldSpecified;

        private decimal chargedBonusField;

        private bool chargedBonusFieldSpecified;

        private decimal chargedStatusBonusField;

        private bool chargedStatusBonusFieldSpecified;

        private decimal writeoffBonusField;

        private bool writeoffBonusFieldSpecified;

        private decimal writeoffStatusBonusField;

        private bool writeoffStatusBonusFieldSpecified;

        private decimal activeChargedBonusField;

        private bool activeChargedBonusFieldSpecified;

        private decimal activeChargedStatusBonusField;

        private bool activeChargedStatusBonusFieldSpecified;

        private ExtendedAttributes[] extendedAttributeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string PositionNumber
        {
            get
            {
                return this.positionNumberField;
            }
            set
            {
                this.positionNumberField = value;
            }
        }

        /// <remarks/>
        public string Article
        {
            get
            {
                return this.articleField;
            }
            set
            {
                this.articleField = value;
            }
        }

        /// <remarks/>
        public string ArticleName
        {
            get
            {
                return this.articleNameField;
            }
            set
            {
                this.articleNameField = value;
            }
        }

        /// <remarks/>
        public decimal Price
        {
            get
            {
                return this.priceField;
            }
            set
            {
                this.priceField = value;
            }
        }

        /// <remarks/>
        public decimal Quantity
        {
            get
            {
                return this.quantityField;
            }
            set
            {
                this.quantityField = value;
            }
        }

        /// <remarks/>
        public decimal Summ
        {
            get
            {
                return this.summField;
            }
            set
            {
                this.summField = value;
            }
        }

        /// <remarks/>
        public decimal Discount
        {
            get
            {
                return this.discountField;
            }
            set
            {
                this.discountField = value;
            }
        }

        /// <remarks/>
        public decimal SummDiscounted
        {
            get
            {
                return this.summDiscountedField;
            }
            set
            {
                this.summDiscountedField = value;
            }
        }

        /// <remarks/>
        public decimal AvailablePayment
        {
            get
            {
                return this.availablePaymentField;
            }
            set
            {
                this.availablePaymentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AvailablePaymentSpecified
        {
            get
            {
                return this.availablePaymentFieldSpecified;
            }
            set
            {
                this.availablePaymentFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal ChargedBonus
        {
            get
            {
                return this.chargedBonusField;
            }
            set
            {
                this.chargedBonusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ChargedBonusSpecified
        {
            get
            {
                return this.chargedBonusFieldSpecified;
            }
            set
            {
                this.chargedBonusFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal ChargedStatusBonus
        {
            get
            {
                return this.chargedStatusBonusField;
            }
            set
            {
                this.chargedStatusBonusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ChargedStatusBonusSpecified
        {
            get
            {
                return this.chargedStatusBonusFieldSpecified;
            }
            set
            {
                this.chargedStatusBonusFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal WriteoffBonus
        {
            get
            {
                return this.writeoffBonusField;
            }
            set
            {
                this.writeoffBonusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool WriteoffBonusSpecified
        {
            get
            {
                return this.writeoffBonusFieldSpecified;
            }
            set
            {
                this.writeoffBonusFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal WriteoffStatusBonus
        {
            get
            {
                return this.writeoffStatusBonusField;
            }
            set
            {
                this.writeoffStatusBonusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool WriteoffStatusBonusSpecified
        {
            get
            {
                return this.writeoffStatusBonusFieldSpecified;
            }
            set
            {
                this.writeoffStatusBonusFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal ActiveChargedBonus
        {
            get
            {
                return this.activeChargedBonusField;
            }
            set
            {
                this.activeChargedBonusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActiveChargedBonusSpecified
        {
            get
            {
                return this.activeChargedBonusFieldSpecified;
            }
            set
            {
                this.activeChargedBonusFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal ActiveChargedStatusBonus
        {
            get
            {
                return this.activeChargedStatusBonusField;
            }
            set
            {
                this.activeChargedStatusBonusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActiveChargedStatusBonusSpecified
        {
            get
            {
                return this.activeChargedStatusBonusFieldSpecified;
            }
            set
            {
                this.activeChargedStatusBonusFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ExtendedAttribute")]
        public ExtendedAttributes[] ExtendedAttribute
        {
            get
            {
                return this.extendedAttributeField;
            }
            set
            {
                this.extendedAttributeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public partial class CreateCardParameter
    {

        private bool createCardField;

        private string iDTemplateField;

        private string cardNumberField;

        private string iDTaskCardField;

        /// <remarks/>
        public bool CreateCard
        {
            get
            {
                return this.createCardField;
            }
            set
            {
                this.createCardField = value;
            }
        }

        /// <remarks/>
        public string IDTemplate
        {
            get
            {
                return this.iDTemplateField;
            }
            set
            {
                this.iDTemplateField = value;
            }
        }

        /// <remarks/>
        public string CardNumber
        {
            get
            {
                return this.cardNumberField;
            }
            set
            {
                this.cardNumberField = value;
            }
        }

        /// <remarks/>
        public string IDTaskCard
        {
            get
            {
                return this.iDTaskCardField;
            }
            set
            {
                this.iDTaskCardField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public partial class Payment
    {

        private string idField;

        private decimal valueField;

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public decimal Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public partial class TransactionReference
    {

        private string transactionIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string TransactionID
        {
            get
            {
                return this.transactionIDField;
            }
            set
            {
                this.transactionIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public partial class ChequeReference
    {

        private string organizationField;

        private string businessUnitField;

        private string pOSField;

        private string numberField;

        private System.DateTime dateTimeField;

        /// <remarks/>
        public string Organization
        {
            get
            {
                return this.organizationField;
            }
            set
            {
                this.organizationField = value;
            }
        }

        /// <remarks/>
        public string BusinessUnit
        {
            get
            {
                return this.businessUnitField;
            }
            set
            {
                this.businessUnitField = value;
            }
        }

        /// <remarks/>
        public string POS
        {
            get
            {
                return this.pOSField;
            }
            set
            {
                this.pOSField = value;
            }
        }

        /// <remarks/>
        public string Number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
            }
        }

        /// <remarks/>
        public System.DateTime DateTime
        {
            get
            {
                return this.dateTimeField;
            }
            set
            {
                this.dateTimeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public partial class WriteOffConfirm
    {

        private bool confirmField;

        private bool confirmFieldSpecified;

        private bool sendCodeField;

        private bool sendCodeFieldSpecified;

        private string verificationCodeField;

        /// <remarks/>
        public bool Confirm
        {
            get
            {
                return this.confirmField;
            }
            set
            {
                this.confirmField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ConfirmSpecified
        {
            get
            {
                return this.confirmFieldSpecified;
            }
            set
            {
                this.confirmFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool SendCode
        {
            get
            {
                return this.sendCodeField;
            }
            set
            {
                this.sendCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SendCodeSpecified
        {
            get
            {
                return this.sendCodeFieldSpecified;
            }
            set
            {
                this.sendCodeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string VerificationCode
        {
            get
            {
                return this.verificationCodeField;
            }
            set
            {
                this.verificationCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public partial class MobilePhone
    {

        private string numberField;

        private bool sendCodeField;

        private bool sendCodeFieldSpecified;

        private string validationCodeField;

        /// <remarks/>
        public string Number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
            }
        }

        /// <remarks/>
        public bool SendCode
        {
            get
            {
                return this.sendCodeField;
            }
            set
            {
                this.sendCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SendCodeSpecified
        {
            get
            {
                return this.sendCodeFieldSpecified;
            }
            set
            {
                this.sendCodeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string ValidationCode
        {
            get
            {
                return this.validationCodeField;
            }
            set
            {
                this.validationCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public partial class BonusRequestBase : RequestBase
    {

        private string numberField;

        private string campaignField;

        private string partnerField;

        private decimal valueField;

        private AwardType_Type awardTypeField;

        private bool awardTypeFieldSpecified;

        private ChargeType_Type chargeTypeField;

        private bool chargeTypeFieldSpecified;

        private AccountingType_Type accountingTypeField;

        private bool accountingTypeFieldSpecified;

        private ActionTime_Type actionTimeField;

        private bool actionTimeFieldSpecified;

        private System.DateTime startDateField;

        private bool startDateFieldSpecified;

        private System.DateTime finishDateField;

        private bool finishDateFieldSpecified;

        private bool isStatusField;

        private bool isStatusFieldSpecified;

        private string descriptionField;

        /// <remarks/>
        public string Number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
            }
        }

        /// <remarks/>
        public string Campaign
        {
            get
            {
                return this.campaignField;
            }
            set
            {
                this.campaignField = value;
            }
        }

        /// <remarks/>
        public string Partner
        {
            get
            {
                return this.partnerField;
            }
            set
            {
                this.partnerField = value;
            }
        }

        /// <remarks/>
        public decimal Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        public AwardType_Type AwardType
        {
            get
            {
                return this.awardTypeField;
            }
            set
            {
                this.awardTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AwardTypeSpecified
        {
            get
            {
                return this.awardTypeFieldSpecified;
            }
            set
            {
                this.awardTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public ChargeType_Type ChargeType
        {
            get
            {
                return this.chargeTypeField;
            }
            set
            {
                this.chargeTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ChargeTypeSpecified
        {
            get
            {
                return this.chargeTypeFieldSpecified;
            }
            set
            {
                this.chargeTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public AccountingType_Type AccountingType
        {
            get
            {
                return this.accountingTypeField;
            }
            set
            {
                this.accountingTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AccountingTypeSpecified
        {
            get
            {
                return this.accountingTypeFieldSpecified;
            }
            set
            {
                this.accountingTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public ActionTime_Type ActionTime
        {
            get
            {
                return this.actionTimeField;
            }
            set
            {
                this.actionTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActionTimeSpecified
        {
            get
            {
                return this.actionTimeFieldSpecified;
            }
            set
            {
                this.actionTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime StartDate
        {
            get
            {
                return this.startDateField;
            }
            set
            {
                this.startDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StartDateSpecified
        {
            get
            {
                return this.startDateFieldSpecified;
            }
            set
            {
                this.startDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime FinishDate
        {
            get
            {
                return this.finishDateField;
            }
            set
            {
                this.finishDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FinishDateSpecified
        {
            get
            {
                return this.finishDateFieldSpecified;
            }
            set
            {
                this.finishDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool isStatus
        {
            get
            {
                return this.isStatusField;
            }
            set
            {
                this.isStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isStatusSpecified
        {
            get
            {
                return this.isStatusFieldSpecified;
            }
            set
            {
                this.isStatusFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public enum AwardType_Type
    {

        /// <remarks/>
        Bonus,

        /// <remarks/>
        Discount,

        /// <remarks/>
        ContactUpdate,
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public enum ChargeType_Type
    {

        /// <remarks/>
        Charge,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Write-off")]
        Writeoff,
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public enum AccountingType_Type
    {

        /// <remarks/>
        Debet,

        /// <remarks/>
        Credit,
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public enum ActionTime_Type
    {

        /// <remarks/>
        Immediately,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("At time")]
        Attime,
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public partial class ContactInfoUpdateRequestBase : RequestBase
    {

        private AwardType_Type awardTypeField;

        private CreateCardParameter[] createCardField;

        /// <remarks/>
        public AwardType_Type AwardType
        {
            get
            {
                return this.awardTypeField;
            }
            set
            {
                this.awardTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CreateCard")]
        public CreateCardParameter[] CreateCard
        {
            get
            {
                return this.createCardField;
            }
            set
            {
                this.createCardField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public partial class CardRequestBase : RequestBase
    {

        private string emailField;

        private string phoneField;

        /// <remarks/>
        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }

        /// <remarks/>
        public string Phone
        {
            get
            {
                return this.phoneField;
            }
            set
            {
                this.phoneField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public partial class Cheque : RequestBase
    {

        private string numberField;

        private OperationType operationTypeField;

        private Payment[] paymentField;

        private decimal summField;

        private decimal discountField;

        private decimal summDiscountedField;

        private decimal paidByBonusField;

        private bool paidByBonusFieldSpecified;

        private decimal paidByStatusBonusField;

        private bool paidByStatusBonusFieldSpecified;

        private Coupon[] couponsField;

        private ExtendedAttributes[] extendedAttributeField;

        private ChequeItem[] itemField;

        private ChequeType chequeTypeField;

        private bool chequeTypeFieldSpecified;

        /// <remarks/>
        public string Number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
            }
        }

        /// <remarks/>
        public OperationType OperationType
        {
            get
            {
                return this.operationTypeField;
            }
            set
            {
                this.operationTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Payment")]
        public Payment[] Payment
        {
            get
            {
                return this.paymentField;
            }
            set
            {
                this.paymentField = value;
            }
        }

        /// <remarks/>
        public decimal Summ
        {
            get
            {
                return this.summField;
            }
            set
            {
                this.summField = value;
            }
        }

        /// <remarks/>
        public decimal Discount
        {
            get
            {
                return this.discountField;
            }
            set
            {
                this.discountField = value;
            }
        }

        /// <remarks/>
        public decimal SummDiscounted
        {
            get
            {
                return this.summDiscountedField;
            }
            set
            {
                this.summDiscountedField = value;
            }
        }

        /// <remarks/>
        public decimal PaidByBonus
        {
            get
            {
                return this.paidByBonusField;
            }
            set
            {
                this.paidByBonusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PaidByBonusSpecified
        {
            get
            {
                return this.paidByBonusFieldSpecified;
            }
            set
            {
                this.paidByBonusFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal PaidByStatusBonus
        {
            get
            {
                return this.paidByStatusBonusField;
            }
            set
            {
                this.paidByStatusBonusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PaidByStatusBonusSpecified
        {
            get
            {
                return this.paidByStatusBonusFieldSpecified;
            }
            set
            {
                this.paidByStatusBonusFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public Coupon[] Coupons
        {
            get
            {
                return this.couponsField;
            }
            set
            {
                this.couponsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ExtendedAttribute")]
        public ExtendedAttributes[] ExtendedAttribute
        {
            get
            {
                return this.extendedAttributeField;
            }
            set
            {
                this.extendedAttributeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Item")]
        public ChequeItem[] Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ChequeType ChequeType
        {
            get
            {
                return this.chequeTypeField;
            }
            set
            {
                this.chequeTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ChequeTypeSpecified
        {
            get
            {
                return this.chequeTypeFieldSpecified;
            }
            set
            {
                this.chequeTypeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public enum OperationType
    {

        /// <remarks/>
        Sale,

        /// <remarks/>
        Return,

        /// <remarks/>
        Rollback,
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]
    public enum ChequeType
    {

        /// <remarks/>
        Fiscal,

        /// <remarks/>
        Soft,
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd", IncludeInSchema = false)]
    public enum ItemsChoiceType
    {

        /// <remarks/>
        BalanceRequest,

        /// <remarks/>
        BonusRequest,

        /// <remarks/>
        CardRequest,

        /// <remarks/>
        ChequeRequest,

        /// <remarks/>
        ContactInfoUpdateRequest,

        /// <remarks/>
        OfferRequest,
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://loyalty.manzanagroup.ru/loyalty.xsd")]    
    public partial class ProcessRequestResponseProcessRequestResult
    {

        private ResponseBase[] itemsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("BalanceResponse", typeof(BalanceResponseBase))]
        [System.Xml.Serialization.XmlElementAttribute("BonusResponse", typeof(ResponseBase))]
        [System.Xml.Serialization.XmlElementAttribute("CardResponse", typeof(CardResponseBase))]
        [System.Xml.Serialization.XmlElementAttribute("ChequeResponse", typeof(ChequeResponseBase))]
        [System.Xml.Serialization.XmlElementAttribute("ContactInfoUpdateResponse", typeof(ContactInfoUpdateResponseBase))]
        [System.Xml.Serialization.XmlElementAttribute("OfferResponse", typeof(OfferResponseBase))]        
        //[System.Xml.Serialization.XmlArrayItem("BalanceResponse", Type = typeof(BalanceResponseBase))]
        //[System.Xml.Serialization.XmlArrayItem("BonusResponse", Type = typeof(ResponseBase))]
        //[System.Xml.Serialization.XmlArrayItem("CardResponse", Type = typeof(CardResponseBase))]
        //[System.Xml.Serialization.XmlArrayItem("ChequeResponse", Type = typeof(ChequeResponseBase))]
        //[System.Xml.Serialization.XmlArrayItem("ContactInfoUpdateResponse", Type = typeof(ContactInfoUpdateResponseBase))]
        //[System.Xml.Serialization.XmlArrayItem("OfferResponse", Type = typeof(OfferResponseBase))] 
        public ResponseBase[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }

}