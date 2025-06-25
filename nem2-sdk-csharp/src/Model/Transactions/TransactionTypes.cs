
using System.ComponentModel;

namespace io.nem2.sdk.Model.Transactions
{
    public static class TransactionTypes
    {       
        public enum Types
        {
            ACCOUNT_KEY_LINK = 0x414C, 
            NODE_KEY_LINK = 0x424C, 
            AGGREGATE_COMPLETE = 0x4141, 
            AGGREGATE_BONDED = 0x4241,
            VOTING_KEY_LINK = 0x4143, 
            VRF_KEY_LINK = 0x4243,
            HASH_LOCK = 0x4148, 
            SECRET_LOCK = 0x4152, 
            SECRET_PROOF = 0x4252, 
            ACCOUNT_METADATA = 0x4144,  
            MOSAIC_METADATA = 0x4244,  
            NAMESPACE_METADATA = 0x4344, 
            MOSAIC_DEFINITION = 0x414D, 
            MOSAIC_SUPPLY_CHANGE = 0x424D,
            MOSAIC_SUPPLY_REVOCATION = 0x434D, 
            MULTISIG_ACCOUNT_MODIFICATION = 0x4155, //-
            ADDRESS_ALIAS = 0x424E, 
            MOSAIC_ALIAS = 0x434E,  
            NAMESPACE_REGISTRATION = 0x414E, 
            ACCOUNT_ADDRESS_RESTRICTION = 0x4150, 
            ACCOUNT_MOSAIC_RESTRICTION = 0x4250, 
            ACCOUNT_OPERATION_RESTRICTION = 0x4350, 
            MOSAIC_ADDRESS_RESTRICTION = 0x4251,
            MOSAIC_GLOBAL_RESTRICTION = 0x4151,
            TRANSFER = 0x4154
        }

        public static ushort GetValue(this Types type)
        {
            if (!Enum.IsDefined(typeof(Types), type))
                throw new InvalidEnumArgumentException(nameof(type), (ushort)type, typeof(Types));

            return (ushort) type;
        }

        public static Types GetRawValue(this ushort type)
        {
            switch (type)
            {
                case 0x4154:
                    return Types.TRANSFER;
                case 0x414e:
                    return Types.NAMESPACE_REGISTRATION;
                case 0x414d:
                    return Types.MOSAIC_DEFINITION;
                case 0x424d:
                    return Types.MOSAIC_SUPPLY_CHANGE;
                case 0x434D:
                    return Types.MOSAIC_SUPPLY_REVOCATION;
                case 0x4155:
                    return Types.MULTISIG_ACCOUNT_MODIFICATION;
                case 0x4141:
                    return Types.AGGREGATE_COMPLETE;
                case 0x4241:
                    return Types.AGGREGATE_BONDED;
                case 0x4148:
                    return Types.HASH_LOCK;
                case 0x4152:
                    return Types.SECRET_LOCK;
                case 0x4252:
                    return Types.SECRET_PROOF;
                case 0x4150:
                    return Types.ACCOUNT_ADDRESS_RESTRICTION;
                case 0x4250:
                    return Types.ACCOUNT_MOSAIC_RESTRICTION;
                case 0x4350:
                    return Types.ACCOUNT_OPERATION_RESTRICTION;
                case 0x4251:
                    return Types.MOSAIC_ADDRESS_RESTRICTION;
                case 0x4151:
                    return Types.MOSAIC_GLOBAL_RESTRICTION;
                case 0x414C:
                    return Types.ACCOUNT_KEY_LINK;
                case 0x424C:
                    return Types.NODE_KEY_LINK;
                case 0x4143:
                    return Types.VOTING_KEY_LINK;
                case 0x4243:
                    return Types.VRF_KEY_LINK;
                case 0x424E:
                    return Types.ADDRESS_ALIAS;
                case 0x434E:
                    return Types.MOSAIC_ALIAS;
                case 0x4144:
                    return Types.ACCOUNT_METADATA;
                case 0x4244:
                    return Types.MOSAIC_METADATA;
                case 0x4344:
                    return Types.NAMESPACE_METADATA;
                default:
                    throw new ArgumentException("invalid transaction type.");
            }
        }
    }
}
