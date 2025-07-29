namespace io.nem2.sdk.src.Model2.Articles.Distinct.Nem
{
    public class Xem : Mosaic
     {
        public static  int Divisibility = 6;

        public static  ulong InitialSupply = 8999999999;

        public static  bool  IsTransferable = true;

        public static  bool IsSupplyMutable = false;

        public static bool IsLevyMutable = false;

        public static  string NamespaceId = "nem";

        public static  MosaicId Id = new MosaicId("nem:xem");

        public Xem(ulong amount) : base(new MosaicId(Id.Id), amount)
        {
                
        }

        public static Xem CreateRelative(ulong amount)
         {
             var relativeAmount = (ulong)Math.Pow(10, Divisibility) * amount;

             return new Xem(relativeAmount);
         }

        public static Xem CreateAbsolute(ulong amount)
         {
             return new Xem(amount);
         }
     }
}
