﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Vault.Integration.ResponseClient.CashConnect {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Safe", Namespace="http://schemas.datacontract.org/2004/07/CCIntegrationLibrary")]
    [System.SerializableAttribute()]
    public partial class Safe : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ErrorsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Vault.Integration.ResponseClient.CashConnect.BagsObject bagsObjectField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string safeIDField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Errors {
            get {
                return this.ErrorsField;
            }
            set {
                if ((object.ReferenceEquals(this.ErrorsField, value) != true)) {
                    this.ErrorsField = value;
                    this.RaisePropertyChanged("Errors");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Vault.Integration.ResponseClient.CashConnect.BagsObject bagsObject {
            get {
                return this.bagsObjectField;
            }
            set {
                if ((object.ReferenceEquals(this.bagsObjectField, value) != true)) {
                    this.bagsObjectField = value;
                    this.RaisePropertyChanged("bagsObject");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string safeID {
            get {
                return this.safeIDField;
            }
            set {
                if ((object.ReferenceEquals(this.safeIDField, value) != true)) {
                    this.safeIDField = value;
                    this.RaisePropertyChanged("safeID");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="BagsObject", Namespace="http://schemas.datacontract.org/2004/07/CCIntegrationLibrary")]
    [System.SerializableAttribute()]
    public partial class BagsObject : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Vault.Integration.ResponseClient.CashConnect.Bag[] BagsField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Vault.Integration.ResponseClient.CashConnect.Bag[] Bags {
            get {
                return this.BagsField;
            }
            set {
                if ((object.ReferenceEquals(this.BagsField, value) != true)) {
                    this.BagsField = value;
                    this.RaisePropertyChanged("Bags");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Bag", Namespace="http://schemas.datacontract.org/2004/07/CCIntegrationLibrary")]
    [System.SerializableAttribute()]
    public partial class Bag : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string bagBarcodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Vault.Integration.ResponseClient.CashConnect.TransactionsObject transactionsObjectField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string bagBarcode {
            get {
                return this.bagBarcodeField;
            }
            set {
                if ((object.ReferenceEquals(this.bagBarcodeField, value) != true)) {
                    this.bagBarcodeField = value;
                    this.RaisePropertyChanged("bagBarcode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Vault.Integration.ResponseClient.CashConnect.TransactionsObject transactionsObject {
            get {
                return this.transactionsObjectField;
            }
            set {
                if ((object.ReferenceEquals(this.transactionsObjectField, value) != true)) {
                    this.transactionsObjectField = value;
                    this.RaisePropertyChanged("transactionsObject");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TransactionsObject", Namespace="http://schemas.datacontract.org/2004/07/CCIntegrationLibrary")]
    [System.SerializableAttribute()]
    public partial class TransactionsObject : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Vault.Integration.ResponseClient.CashConnect.Transaction[] TransactionsField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Vault.Integration.ResponseClient.CashConnect.Transaction[] Transactions {
            get {
                return this.TransactionsField;
            }
            set {
                if ((object.ReferenceEquals(this.TransactionsField, value) != true)) {
                    this.TransactionsField = value;
                    this.RaisePropertyChanged("Transactions");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Transaction", Namespace="http://schemas.datacontract.org/2004/07/CCIntegrationLibrary")]
    [System.SerializableAttribute()]
    public partial class Transaction : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double amountField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int bagNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int dropNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string empNoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime trasanctionDateField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double amount {
            get {
                return this.amountField;
            }
            set {
                if ((this.amountField.Equals(value) != true)) {
                    this.amountField = value;
                    this.RaisePropertyChanged("amount");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int bagNumber {
            get {
                return this.bagNumberField;
            }
            set {
                if ((this.bagNumberField.Equals(value) != true)) {
                    this.bagNumberField = value;
                    this.RaisePropertyChanged("bagNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int dropNumber {
            get {
                return this.dropNumberField;
            }
            set {
                if ((this.dropNumberField.Equals(value) != true)) {
                    this.dropNumberField = value;
                    this.RaisePropertyChanged("dropNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string empNo {
            get {
                return this.empNoField;
            }
            set {
                if ((object.ReferenceEquals(this.empNoField, value) != true)) {
                    this.empNoField = value;
                    this.RaisePropertyChanged("empNo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime trasanctionDate {
            get {
                return this.trasanctionDateField;
            }
            set {
                if ((this.trasanctionDateField.Equals(value) != true)) {
                    this.trasanctionDateField = value;
                    this.RaisePropertyChanged("trasanctionDate");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="CashConnect.ICashConnect")]
    public interface ICashConnect {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICashConnect/CountInfoReceive", ReplyAction="http://tempuri.org/ICashConnect/CountInfoReceiveResponse")]
        string CountInfoReceive(string Safe_ID, string Bag_Barcode, System.DateTime Date, int Declared10, int Declared20, int Declared50, int Declared100, int Declared200, float DeclaredValue, int Counted10, int Counted20, int Counted50, int Counted100, int Counted200, float CountedValue);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICashConnect/CountInfoReceive", ReplyAction="http://tempuri.org/ICashConnect/CountInfoReceiveResponse")]
        System.Threading.Tasks.Task<string> CountInfoReceiveAsync(string Safe_ID, string Bag_Barcode, System.DateTime Date, int Declared10, int Declared20, int Declared50, int Declared100, int Declared200, float DeclaredValue, int Counted10, int Counted20, int Counted50, int Counted100, int Counted200, float CountedValue);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICashConnect/Get_Safe_Transation_List", ReplyAction="http://tempuri.org/ICashConnect/Get_Safe_Transation_ListResponse")]
        Vault.Integration.ResponseClient.CashConnect.Safe Get_Safe_Transation_List(string SafeID, System.DateTime dateFrom, string userName, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICashConnect/Get_Safe_Transation_List", ReplyAction="http://tempuri.org/ICashConnect/Get_Safe_Transation_ListResponse")]
        System.Threading.Tasks.Task<Vault.Integration.ResponseClient.CashConnect.Safe> Get_Safe_Transation_ListAsync(string SafeID, System.DateTime dateFrom, string userName, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICashConnect/safe", ReplyAction="http://tempuri.org/ICashConnect/safeResponse")]
        Vault.Integration.ResponseClient.CashConnect.Safe safe();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICashConnect/safe", ReplyAction="http://tempuri.org/ICashConnect/safeResponse")]
        System.Threading.Tasks.Task<Vault.Integration.ResponseClient.CashConnect.Safe> safeAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICashConnect/bagsObject", ReplyAction="http://tempuri.org/ICashConnect/bagsObjectResponse")]
        Vault.Integration.ResponseClient.CashConnect.BagsObject bagsObject();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICashConnect/bagsObject", ReplyAction="http://tempuri.org/ICashConnect/bagsObjectResponse")]
        System.Threading.Tasks.Task<Vault.Integration.ResponseClient.CashConnect.BagsObject> bagsObjectAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICashConnect/bag", ReplyAction="http://tempuri.org/ICashConnect/bagResponse")]
        Vault.Integration.ResponseClient.CashConnect.Bag bag();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICashConnect/bag", ReplyAction="http://tempuri.org/ICashConnect/bagResponse")]
        System.Threading.Tasks.Task<Vault.Integration.ResponseClient.CashConnect.Bag> bagAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICashConnect/transactionsObject", ReplyAction="http://tempuri.org/ICashConnect/transactionsObjectResponse")]
        Vault.Integration.ResponseClient.CashConnect.TransactionsObject transactionsObject();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICashConnect/transactionsObject", ReplyAction="http://tempuri.org/ICashConnect/transactionsObjectResponse")]
        System.Threading.Tasks.Task<Vault.Integration.ResponseClient.CashConnect.TransactionsObject> transactionsObjectAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICashConnect/transaction", ReplyAction="http://tempuri.org/ICashConnect/transactionResponse")]
        Vault.Integration.ResponseClient.CashConnect.Transaction transaction();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICashConnect/transaction", ReplyAction="http://tempuri.org/ICashConnect/transactionResponse")]
        System.Threading.Tasks.Task<Vault.Integration.ResponseClient.CashConnect.Transaction> transactionAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICashConnectChannel : Vault.Integration.ResponseClient.CashConnect.ICashConnect, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CashConnectClient : System.ServiceModel.ClientBase<Vault.Integration.ResponseClient.CashConnect.ICashConnect>, Vault.Integration.ResponseClient.CashConnect.ICashConnect {
        
        public CashConnectClient() {
        }
        
        public CashConnectClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CashConnectClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CashConnectClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CashConnectClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string CountInfoReceive(string Safe_ID, string Bag_Barcode, System.DateTime Date, int Declared10, int Declared20, int Declared50, int Declared100, int Declared200, float DeclaredValue, int Counted10, int Counted20, int Counted50, int Counted100, int Counted200, float CountedValue) {
            return base.Channel.CountInfoReceive(Safe_ID, Bag_Barcode, Date, Declared10, Declared20, Declared50, Declared100, Declared200, DeclaredValue, Counted10, Counted20, Counted50, Counted100, Counted200, CountedValue);
        }
        
        public System.Threading.Tasks.Task<string> CountInfoReceiveAsync(string Safe_ID, string Bag_Barcode, System.DateTime Date, int Declared10, int Declared20, int Declared50, int Declared100, int Declared200, float DeclaredValue, int Counted10, int Counted20, int Counted50, int Counted100, int Counted200, float CountedValue) {
            return base.Channel.CountInfoReceiveAsync(Safe_ID, Bag_Barcode, Date, Declared10, Declared20, Declared50, Declared100, Declared200, DeclaredValue, Counted10, Counted20, Counted50, Counted100, Counted200, CountedValue);
        }
        
        public Vault.Integration.ResponseClient.CashConnect.Safe Get_Safe_Transation_List(string SafeID, System.DateTime dateFrom, string userName, string password) {
            return base.Channel.Get_Safe_Transation_List(SafeID, dateFrom, userName, password);
        }
        
        public System.Threading.Tasks.Task<Vault.Integration.ResponseClient.CashConnect.Safe> Get_Safe_Transation_ListAsync(string SafeID, System.DateTime dateFrom, string userName, string password) {
            return base.Channel.Get_Safe_Transation_ListAsync(SafeID, dateFrom, userName, password);
        }
        
        public Vault.Integration.ResponseClient.CashConnect.Safe safe() {
            return base.Channel.safe();
        }
        
        public System.Threading.Tasks.Task<Vault.Integration.ResponseClient.CashConnect.Safe> safeAsync() {
            return base.Channel.safeAsync();
        }
        
        public Vault.Integration.ResponseClient.CashConnect.BagsObject bagsObject() {
            return base.Channel.bagsObject();
        }
        
        public System.Threading.Tasks.Task<Vault.Integration.ResponseClient.CashConnect.BagsObject> bagsObjectAsync() {
            return base.Channel.bagsObjectAsync();
        }
        
        public Vault.Integration.ResponseClient.CashConnect.Bag bag() {
            return base.Channel.bag();
        }
        
        public System.Threading.Tasks.Task<Vault.Integration.ResponseClient.CashConnect.Bag> bagAsync() {
            return base.Channel.bagAsync();
        }
        
        public Vault.Integration.ResponseClient.CashConnect.TransactionsObject transactionsObject() {
            return base.Channel.transactionsObject();
        }
        
        public System.Threading.Tasks.Task<Vault.Integration.ResponseClient.CashConnect.TransactionsObject> transactionsObjectAsync() {
            return base.Channel.transactionsObjectAsync();
        }
        
        public Vault.Integration.ResponseClient.CashConnect.Transaction transaction() {
            return base.Channel.transaction();
        }
        
        public System.Threading.Tasks.Task<Vault.Integration.ResponseClient.CashConnect.Transaction> transactionAsync() {
            return base.Channel.transactionAsync();
        }
    }
}
