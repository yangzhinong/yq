 
using  LoticLight.Model;

namespace LoticLight.Repository
{
   
	

	public partial class Sms_DataDictionaryRepository :BaseRepository<Sms_DataDictionary>
    {
	 static Sms_DataDictionaryRepository _instance;
        public static Sms_DataDictionaryRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new Sms_DataDictionaryRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class Sms_SettingRepository :BaseRepository<Sms_Setting>
    {
	 static Sms_SettingRepository _instance;
        public static Sms_SettingRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new Sms_SettingRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class Sys_ActionRepository :BaseRepository<Sys_Action>
    {
	 static Sys_ActionRepository _instance;
        public static Sys_ActionRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new Sys_ActionRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class Sys_DepartmentRepository :BaseRepository<Sys_Department>
    {
	 static Sys_DepartmentRepository _instance;
        public static Sys_DepartmentRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new Sys_DepartmentRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class Sys_GridViewRepository :BaseRepository<Sys_GridView>
    {
	 static Sys_GridViewRepository _instance;
        public static Sys_GridViewRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new Sys_GridViewRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class Sys_LogRepository :BaseRepository<Sys_Log>
    {
	 static Sys_LogRepository _instance;
        public static Sys_LogRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new Sys_LogRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class Sys_MenuRepository :BaseRepository<Sys_Menu>
    {
	 static Sys_MenuRepository _instance;
        public static Sys_MenuRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new Sys_MenuRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class Sys_RelationRepository :BaseRepository<Sys_Relation>
    {
	 static Sys_RelationRepository _instance;
        public static Sys_RelationRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new Sys_RelationRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class Sys_RoleRepository :BaseRepository<Sys_Role>
    {
	 static Sys_RoleRepository _instance;
        public static Sys_RoleRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new Sys_RoleRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class Sys_UserRepository :BaseRepository<Sys_User>
    {
	 static Sys_UserRepository _instance;
        public static Sys_UserRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new Sys_UserRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class Sys_UserTokenRepository :BaseRepository<Sys_UserToken>
    {
	 static Sys_UserTokenRepository _instance;
        public static Sys_UserTokenRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new Sys_UserTokenRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class sysdiagramsRepository :BaseRepository<sysdiagrams>
    {
	 static sysdiagramsRepository _instance;
        public static sysdiagramsRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new sysdiagramsRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class YqGSMRepository :BaseRepository<YqGSM>
    {
	 static YqGSMRepository _instance;
        public static YqGSMRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new YqGSMRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class YqGSMFeedBackRepository :BaseRepository<YqGSMFeedBack>
    {
	 static YqGSMFeedBackRepository _instance;
        public static YqGSMFeedBackRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new YqGSMFeedBackRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class YqLocationRepository :BaseRepository<YqLocation>
    {
	 static YqLocationRepository _instance;
        public static YqLocationRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new YqLocationRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class YqNPSARepository :BaseRepository<YqNPSA>
    {
	 static YqNPSARepository _instance;
        public static YqNPSARepository Instance
        {
            get
            {
                if (_instance == null)
                    return new YqNPSARepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class YqNPSADataRepository :BaseRepository<YqNPSAData>
    {
	 static YqNPSADataRepository _instance;
        public static YqNPSADataRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new YqNPSADataRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class YqNPSAInputDataRepository :BaseRepository<YqNPSAInputData>
    {
	 static YqNPSAInputDataRepository _instance;
        public static YqNPSAInputDataRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new YqNPSAInputDataRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class YqPNRepository :BaseRepository<YqPN>
    {
	 static YqPNRepository _instance;
        public static YqPNRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new YqPNRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class YqPnLocationRepository :BaseRepository<YqPnLocation>
    {
	 static YqPnLocationRepository _instance;
        public static YqPnLocationRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new YqPnLocationRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class YqPnProjectRepository :BaseRepository<YqPnProject>
    {
	 static YqPnProjectRepository _instance;
        public static YqPnProjectRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new YqPnProjectRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class YqPnQtyRepository :BaseRepository<YqPnQty>
    {
	 static YqPnQtyRepository _instance;
        public static YqPnQtyRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new YqPnQtyRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class YqProjectRepository :BaseRepository<YqProject>
    {
	 static YqProjectRepository _instance;
        public static YqProjectRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new YqProjectRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class YqSBBTypeRepository :BaseRepository<YqSBBType>
    {
	 static YqSBBTypeRepository _instance;
        public static YqSBBTypeRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new YqSBBTypeRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class YqVendorRepository :BaseRepository<YqVendor>
    {
	 static YqVendorRepository _instance;
        public static YqVendorRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new YqVendorRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class YqViewNPSADataRepository :BaseRepository<YqViewNPSAData>
    {
	 static YqViewNPSADataRepository _instance;
        public static YqViewNPSADataRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new YqViewNPSADataRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class YqViewNPSAInputDataRepository :BaseRepository<YqViewNPSAInputData>
    {
	 static YqViewNPSAInputDataRepository _instance;
        public static YqViewNPSAInputDataRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new YqViewNPSAInputDataRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class YqViewPNRepository :BaseRepository<YqViewPN>
    {
	 static YqViewPNRepository _instance;
        public static YqViewPNRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new YqViewPNRepository();
                else
                    return _instance;
            }
        }
         
    }

	

	public partial class YqWeekRepository :BaseRepository<YqWeek>
    {
	 static YqWeekRepository _instance;
        public static YqWeekRepository Instance
        {
            get
            {
                if (_instance == null)
                    return new YqWeekRepository();
                else
                    return _instance;
            }
        }
         
    }

	
}