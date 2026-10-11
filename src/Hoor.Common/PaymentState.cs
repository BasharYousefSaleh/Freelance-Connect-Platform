namespace Hoor.Common
{
    public enum PaymentState
    {
    //حالات الدفع
    //فوق كل حالة تعليق يوضح ترجمتها
        //قيد التنفيذ
        Pending = 1,

        //تم التنفيذ
        succeeded = 2,
        //فشل التنفيذ
        Failed = 3,
        //ملغي
        Canceled = 4,

        //ارجاع
        Refunded = 5,

        //انتهى التنفيذ
        Expired = 6,
        
        Released = 7,

    }
}
