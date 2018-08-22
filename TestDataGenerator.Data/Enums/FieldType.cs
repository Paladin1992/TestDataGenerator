using System.ComponentModel;

namespace TestDataGenerator.Data.Enums
{
    // mindegyiknél legyen:
    //      checkbox: lehessen NULL is x% eséllyel
    //      checkbox: intervallum
    //          gomb: alapértelmezett (confirm)

    /*
    dátum és idő:           0001.01.01 00:00:00 - 9999.12.31 23:59:59

    egész szám (32 bites):
   	             előjeles:	min(int)  - max(int)
           előjel nélküli:  min(uint) - max(uint)

    egész szám (64 bites):
	             előjeles:	min(long)  - max(long)
           előjel nélküli:	min(ulong) - max(ulong)

    valós szám (32 bites):	min(float) - max(float)

    valós szám (64 bites):	min(double) - max(double)
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
        LastName,

        /// <summary>
        /// Csak keresztnevet fog generálni.
        /// </summary>
        [Description("Keresztnév")]
        FirstName,

        /// <summary>
        /// Egy dátumot fog generálni a megadott dátumok között.
        /// (Tipp: be lehessen állítani, hogy mindig a mai dátum legyen valamelyik határ, mondjuk radio gombbal:
        /// * mai dátum, * egyéb: éééé.hh.nn)
        /// </summary>
        [Description("Dátum és idő")]
        [HasExtreme(HasMinValue = true, HasMaxValue = true)]
        DateTime,

        /// <summary>
        /// A szabványos e-mail cím reguláris kifejezésének megfelelő szöveget generál.
        /// </summary>
        [Description("E-mail cím")]
        Email,

        /// <summary>
        /// Egy megadott paraméterek szerinti karaktersorozatot generál, adott hosszal.
        /// (Tippek: radio gombokkal lehessen állítani, hogy kisbetűs, nagybetűs vagy pedig vegyes legyen-e a szöveg;
        /// checkbox: legyen benne szóköz;
        /// checkbox: legyen benne szám;
        /// checkbox: legyen benne ékezetes karakter;
        /// checkbox: legyen benne: [input])
        /// </summary>
        [Description("Szöveg")]
        [HasExtreme(HasMinValue = true, HasMaxValue = true)]
        Text,

        /// <summary>
        /// Egy 32 bites egész számot generál.
        /// (Tipp: checkbox: előjeles)
        /// </summary>
        [Description("Egész szám (32 bites)")]
        [HasExtreme(HasMinValue = true, HasMaxValue = true)]
        Integer,

        /// <summary>
        /// Egy 64 bites egész számot generál.
        /// (Tipp: checkbox: előjeles)
        /// </summary>
        [Description("Egész szám (64 bites)")]
        [HasExtreme(HasMinValue = true, HasMaxValue = true)]
        LongInteger,

        /// <summary>
        /// Egy 32 bites lebegőpontos számot generál.
        /// </summary>
        [Description("Valós szám (32 bites)")]
        [HasExtreme(HasMinValue = true, HasMaxValue = true)]
        Float,

        /// <summary>
        /// Egy 64 bites lebegőpontos számot generál.
        /// </summary>
        [Description("Valós szám (64 bites)")]
        [HasExtreme(HasMinValue = true, HasMaxValue = true)]
        Double,

        /// <summary>
        /// Egy adott hosszúságú hash-szerű szöveget generál, melyben csak hexadecimális karakterek vannak.
        /// </summary>
        [Description("Hash")]
        [HasExtreme(HasMinValue = true, HasMaxValue = true)]
        Hash,

        /// <summary>
        /// Egy GUID-ot generál a megadott formátum szerint.
        /// (Tipp: checkbox: kötőjelekkel)
        /// </summary>
        [Description("GUID")]
        Guid,

        //[Description("Base64 szöveg")]
        //[HasExtreme(HasMinValue = true, HasMaxValue = true)]
        //Base64,

        /// <summary>
        /// A megadott értékkészletből választ véletlenszerűen egy értéket.
        /// Az egyes elemeket a | karakterrel lehet elválasztani. Pl.: "tér|út|utca"
        /// </summary>
        [Description("Fix értékkészlet")]
        CustomSet
    }
}