using System;

namespace RoomFinishing
{
    internal class CustomUnitUtils
    {
        private const double METERS_IN_FEET = 0.3048;

        internal static double ConvertFromInternalUnits(double value, UnitType unit)
        {
            switch (unit)
            {
                #region Length

                case UnitType.MILLIMETERS:
                    return value * (METERS_IN_FEET * 1000);
                case UnitType.CENTIMETERS:
                    return value * (METERS_IN_FEET * 100);
                case UnitType.METERS:
                    return value * METERS_IN_FEET;

                #endregion

                #region Area

                case UnitType.SQUARE_MILLIMETERS:
                    return value * Math.Pow(METERS_IN_FEET * 1000, 2);
                case UnitType.SQUARE_CENTIMETERS:
                    return value * Math.Pow(METERS_IN_FEET * 100, 2);
                case UnitType.SQUARE_METERS:
                    return value * Math.Pow(METERS_IN_FEET, 2);

                #endregion

                #region Volume

                case UnitType.CUBIC_MILLIMETERS:
                    return value * Math.Pow(METERS_IN_FEET * 1000, 3);
                case UnitType.CUBIC_CENTIMETERS:
                    return value * Math.Pow(METERS_IN_FEET * 100, 3);
                case UnitType.CUBIC_METERS:
                    return value * Math.Pow(METERS_IN_FEET, 3);

                #endregion

                default:
                    throw new NotSupportedException("Not supported type");
            }
        }

        internal static double ConvertToInternalUnits(double value, UnitType unit)
        {
            switch (unit)
            {
                #region Length

                case UnitType.MILLIMETERS:
                    return value / (METERS_IN_FEET * 1000);
                case UnitType.CENTIMETERS:
                    return value / (METERS_IN_FEET * 100);
                case UnitType.METERS:
                    return value / METERS_IN_FEET;

                #endregion

                #region Area

                case UnitType.SQUARE_MILLIMETERS:
                    return value / Math.Pow(METERS_IN_FEET * 1000, 2);
                case UnitType.SQUARE_CENTIMETERS:
                    return value / Math.Pow(METERS_IN_FEET * 100, 2);
                case UnitType.SQUARE_METERS:
                    return value / Math.Pow(METERS_IN_FEET, 2);

                #endregion

                #region Volume

                case UnitType.CUBIC_MILLIMETERS:
                    return value / Math.Pow(METERS_IN_FEET * 1000, 3);
                case UnitType.CUBIC_CENTIMETERS:
                    return value / Math.Pow(METERS_IN_FEET * 100, 3);
                case UnitType.CUBIC_METERS:
                    return value / Math.Pow(METERS_IN_FEET, 3);

                #endregion

                default:
                    throw new NotSupportedException("Not supported type");
            }
        }
    }

    internal enum UnitType
    {
        #region Length

        MILLIMETERS,
        CENTIMETERS,
        METERS,

        #endregion

        #region Area

        SQUARE_MILLIMETERS,
        SQUARE_CENTIMETERS,
        SQUARE_METERS,

        #endregion

        #region Volume

        CUBIC_MILLIMETERS,
        CUBIC_CENTIMETERS,
        CUBIC_METERS

        #endregion
    }
}
