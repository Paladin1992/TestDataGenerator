using System.ComponentModel;

namespace TestDataGenerator.Data.Enums
{
    // mindegyiknél legyen:
    //      checkbox: lehessen NULL is x% eséllyel
    //      checkbox: intervallum
    //          gomb: alapértelmezett (confirm)

    /*
    dátum és idő:           0001.01.01 00:00:00 - 9999.12.31 23:59:59

    egész szám (8 bites):
   	             előjeles:	min(byte)  - max(byte)
           előjel nélküli:  min(sbyte) - max(sbyte)

    egész szám (16 bites):
   	             előjeles:	min(short)  - max(short)
           előjel nélküli:  min(ushort) - max(ushort)

    egész szám (32 bites):
   	             előjeles:	min(int)  - max(int)
           előjel nélküli:  min(uint) - max(uint)

    egész szám (64 bites):
	             előjeles:	min(long)  - max(long)
           előjel nélküli:	min(ulong) - max(ulong)

    valós szám (32 bites):	min(float) - max(float)

    valós szám (64 bites):	min(double) - max(double)

    decimális (128 bites):  min(decimal) - max(decimal)
    */

    public enum FieldType
    {
        /// <summary>
        /// Nincs kiválasztva mezőtípus.
        /// </summary>
        [Description("(Nincs)")]
        None = 0,

        /// <summary>
        /// Csak vezetéknevet fog generálni.
        /// </summary>
        [Description("Vezetéknév")]
        LastName = 1,

        /// <summary>
        /// Csak keresztnevet fog generálni.
        /// </summary>
        [Description("Keresztnév")]
        FirstName = 2,

        /// <summary>
        /// Egy dátumot fog generálni a megadott dátumok között.
        /// (Tipp: be lehessen állítani, hogy mindig a mai dátum legyen valamelyik határ, mondjuk radio gombbal:
        /// * mai dátum, * egyéb: éééé.hh.nn)
        /// </summary>
        [Description("Dátum és idő")]
        [HasExtreme(HasMinValue = true, HasMaxValue = true)]
        DateTime = 3,

        /// <summary>
        /// A szabványos e-mail cím reguláris kifejezésének megfelelő szöveget generál.
        /// </summary>
        [Description("E-mail cím")]
        Email = 4,

        /// <summary>
        /// Egy megadott paraméterek szerinti karaktersorozatot generál, adott hosszal.
        /// (Tippek: radio gombokkal lehessen állítani, hogy kisbetűs, nagybetűs vagy pedig vegyes legyen-e a szöveg;
        /// checkbox: legyen benne szám;
        /// checkbox: legyen benne: [input])
        /// </summary>
        [Description("Szöveg")]
        [HasExtreme(HasMinValue = true, HasMaxValue = true)]
        Text = 5,

        /// <summary>
        /// Egy 8 bites egész számot generál.
        /// (Tipp: checkbox: előjeles)
        /// </summary>
        [Description("Egész szám (8 bites)")]
        [HasExtreme(HasMinValue = true, HasMaxValue = true)]
        Byte = 6,

        /// <summary>
        /// Egy 16 bites egész számot generál.
        /// (Tipp: checkbox: előjeles)
        /// </summary>
        [Description("Egész szám (16 bites)")]
        [HasExtreme(HasMinValue = true, HasMaxValue = true)]
        Int16 = 7,

        /// <summary>
        /// Egy 32 bites egész számot generál.
        /// (Tipp: checkbox: előjeles)
        /// </summary>
        [Description("Egész szám (32 bites)")]
        [HasExtreme(HasMinValue = true, HasMaxValue = true)]
        Int32 = 8,

        /// <summary>
        /// Egy 64 bites egész számot generál.
        /// (Tipp: checkbox: előjeles)
        /// </summary>
        [Description("Egész szám (64 bites)")]
        [HasExtreme(HasMinValue = true, HasMaxValue = true)]
        Int64 = 9,

        /// <summary>
        /// Egy 32 bites lebegőpontos számot generál.
        /// </summary>
        [Description("Valós szám (32 bites lebegőpontos)")]
        [HasExtreme(HasMinValue = true, HasMaxValue = true)]
        Single = 10,

        /// <summary>
        /// Egy 64 bites lebegőpontos számot generál.
        /// </summary>
        [Description("Valós szám (64 bites lebegőpontos)")]
        [HasExtreme(HasMinValue = true, HasMaxValue = true)]
        Double = 11,

        /// <summary>
        /// Egy 128 bites decimális számot generál.
        /// </summary>
        [Description("Decimális (128 bites)")]
        [HasExtreme(HasMinValue = true, HasMaxValue = true)]
        Decimal = 12,

        /// <summary>
        /// Egy adott hosszúságú hash-szerű szöveget generál, melyben csak hexadecimális karakterek vannak.
        /// </summary>
        [Description("Hash")]
        Hash = 13,

        /// <summary>
        /// Egy GUID-ot generál a megadott formátum szerint.
        /// </summary>
        [Description("GUID")]
        Guid = 14,

        /// <summary>
        /// Egy adott hosszúságú szöveget generál Base64-es kódolással.
        /// </summary>
        [Description("Base64 szöveg")]
        Base64 = 15,

        /// <summary>
        /// A megadott értékkészletből választ véletlenszerűen egy értéket.
        /// Az egyes elemeket a | karakterrel lehet elválasztani. Pl.: "tér|út|utca"
        /// (A felületen lenne egy lista hozzá, de azt végül | karakterekkel tagolt stringként küldené fel a szerver felé.)
        /// </summary>
        [Description("Fix értékkészlet")]
        CustomSet = 16
    }
}