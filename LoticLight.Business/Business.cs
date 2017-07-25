 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoticLight.Repository;
using LoticLight.Model;

namespace LoticLight.Business
{
	
	public partial class Sms_DataDictionaryService:BaseService<Sms_DataDictionary>
    { 


	 static Sms_DataDictionaryService _instance;
        public static Sms_DataDictionaryService Instance
        {
            get
            {
                if (_instance == null)
                    return new Sms_DataDictionaryService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.Sms_DataDictionaryRepository;
        }  
    }
	
	public partial class Sms_SettingService:BaseService<Sms_Setting>
    { 


	 static Sms_SettingService _instance;
        public static Sms_SettingService Instance
        {
            get
            {
                if (_instance == null)
                    return new Sms_SettingService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.Sms_SettingRepository;
        }  
    }
	
	public partial class Sys_ActionService:BaseService<Sys_Action>
    { 


	 static Sys_ActionService _instance;
        public static Sys_ActionService Instance
        {
            get
            {
                if (_instance == null)
                    return new Sys_ActionService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.Sys_ActionRepository;
        }  
    }
	
	public partial class Sys_DepartmentService:BaseService<Sys_Department>
    { 


	 static Sys_DepartmentService _instance;
        public static Sys_DepartmentService Instance
        {
            get
            {
                if (_instance == null)
                    return new Sys_DepartmentService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.Sys_DepartmentRepository;
        }  
    }
	
	public partial class Sys_GridViewService:BaseService<Sys_GridView>
    { 


	 static Sys_GridViewService _instance;
        public static Sys_GridViewService Instance
        {
            get
            {
                if (_instance == null)
                    return new Sys_GridViewService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.Sys_GridViewRepository;
        }  
    }
	
	public partial class Sys_LogService:BaseService<Sys_Log>
    { 


	 static Sys_LogService _instance;
        public static Sys_LogService Instance
        {
            get
            {
                if (_instance == null)
                    return new Sys_LogService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.Sys_LogRepository;
        }  
    }
	
	public partial class Sys_MenuService:BaseService<Sys_Menu>
    { 


	 static Sys_MenuService _instance;
        public static Sys_MenuService Instance
        {
            get
            {
                if (_instance == null)
                    return new Sys_MenuService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.Sys_MenuRepository;
        }  
    }
	
	public partial class Sys_RelationService:BaseService<Sys_Relation>
    { 


	 static Sys_RelationService _instance;
        public static Sys_RelationService Instance
        {
            get
            {
                if (_instance == null)
                    return new Sys_RelationService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.Sys_RelationRepository;
        }  
    }
	
	public partial class Sys_RoleService:BaseService<Sys_Role>
    { 


	 static Sys_RoleService _instance;
        public static Sys_RoleService Instance
        {
            get
            {
                if (_instance == null)
                    return new Sys_RoleService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.Sys_RoleRepository;
        }  
    }
	
	public partial class Sys_UserService:BaseService<Sys_User>
    { 


	 static Sys_UserService _instance;
        public static Sys_UserService Instance
        {
            get
            {
                if (_instance == null)
                    return new Sys_UserService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.Sys_UserRepository;
        }  
    }
	
	public partial class Sys_UserTokenService:BaseService<Sys_UserToken>
    { 


	 static Sys_UserTokenService _instance;
        public static Sys_UserTokenService Instance
        {
            get
            {
                if (_instance == null)
                    return new Sys_UserTokenService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.Sys_UserTokenRepository;
        }  
    }
	
	public partial class sysdiagramsService:BaseService<sysdiagrams>
    { 


	 static sysdiagramsService _instance;
        public static sysdiagramsService Instance
        {
            get
            {
                if (_instance == null)
                    return new sysdiagramsService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.sysdiagramsRepository;
        }  
    }
	
	public partial class YqGSMService:BaseService<YqGSM>
    { 


	 static YqGSMService _instance;
        public static YqGSMService Instance
        {
            get
            {
                if (_instance == null)
                    return new YqGSMService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.YqGSMRepository;
        }  
    }
	
	public partial class YqGSMFeedBackService:BaseService<YqGSMFeedBack>
    { 


	 static YqGSMFeedBackService _instance;
        public static YqGSMFeedBackService Instance
        {
            get
            {
                if (_instance == null)
                    return new YqGSMFeedBackService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.YqGSMFeedBackRepository;
        }  
    }
	
	public partial class YqLocationService:BaseService<YqLocation>
    { 


	 static YqLocationService _instance;
        public static YqLocationService Instance
        {
            get
            {
                if (_instance == null)
                    return new YqLocationService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.YqLocationRepository;
        }  
    }
	
	public partial class YqNPSAService:BaseService<YqNPSA>
    { 


	 static YqNPSAService _instance;
        public static YqNPSAService Instance
        {
            get
            {
                if (_instance == null)
                    return new YqNPSAService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.YqNPSARepository;
        }  
    }
	
	public partial class YqNPSADataService:BaseService<YqNPSAData>
    { 


	 static YqNPSADataService _instance;
        public static YqNPSADataService Instance
        {
            get
            {
                if (_instance == null)
                    return new YqNPSADataService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.YqNPSADataRepository;
        }  
    }
	
	public partial class YqNPSAInputDataService:BaseService<YqNPSAInputData>
    { 


	 static YqNPSAInputDataService _instance;
        public static YqNPSAInputDataService Instance
        {
            get
            {
                if (_instance == null)
                    return new YqNPSAInputDataService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.YqNPSAInputDataRepository;
        }  
    }
	
	public partial class YqPNService:BaseService<YqPN>
    { 


	 static YqPNService _instance;
        public static YqPNService Instance
        {
            get
            {
                if (_instance == null)
                    return new YqPNService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.YqPNRepository;
        }  
    }
	
	public partial class YqPnLocationService:BaseService<YqPnLocation>
    { 


	 static YqPnLocationService _instance;
        public static YqPnLocationService Instance
        {
            get
            {
                if (_instance == null)
                    return new YqPnLocationService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.YqPnLocationRepository;
        }  
    }
	
	public partial class YqPnProjectService:BaseService<YqPnProject>
    { 


	 static YqPnProjectService _instance;
        public static YqPnProjectService Instance
        {
            get
            {
                if (_instance == null)
                    return new YqPnProjectService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.YqPnProjectRepository;
        }  
    }
	
	public partial class YqPnQtyService:BaseService<YqPnQty>
    { 


	 static YqPnQtyService _instance;
        public static YqPnQtyService Instance
        {
            get
            {
                if (_instance == null)
                    return new YqPnQtyService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.YqPnQtyRepository;
        }  
    }
	
	public partial class YqProjectService:BaseService<YqProject>
    { 


	 static YqProjectService _instance;
        public static YqProjectService Instance
        {
            get
            {
                if (_instance == null)
                    return new YqProjectService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.YqProjectRepository;
        }  
    }
	
	public partial class YqSBBTypeService:BaseService<YqSBBType>
    { 


	 static YqSBBTypeService _instance;
        public static YqSBBTypeService Instance
        {
            get
            {
                if (_instance == null)
                    return new YqSBBTypeService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.YqSBBTypeRepository;
        }  
    }
	
	public partial class YqVendorService:BaseService<YqVendor>
    { 


	 static YqVendorService _instance;
        public static YqVendorService Instance
        {
            get
            {
                if (_instance == null)
                    return new YqVendorService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.YqVendorRepository;
        }  
    }
	
	public partial class YqViewNPSADataService:BaseService<YqViewNPSAData>
    { 


	 static YqViewNPSADataService _instance;
        public static YqViewNPSADataService Instance
        {
            get
            {
                if (_instance == null)
                    return new YqViewNPSADataService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.YqViewNPSADataRepository;
        }  
    }
	
	public partial class YqViewNPSAInputDataService:BaseService<YqViewNPSAInputData>
    { 


	 static YqViewNPSAInputDataService _instance;
        public static YqViewNPSAInputDataService Instance
        {
            get
            {
                if (_instance == null)
                    return new YqViewNPSAInputDataService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.YqViewNPSAInputDataRepository;
        }  
    }
	
	public partial class YqViewPNService:BaseService<YqViewPN>
    { 


	 static YqViewPNService _instance;
        public static YqViewPNService Instance
        {
            get
            {
                if (_instance == null)
                    return new YqViewPNService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.YqViewPNRepository;
        }  
    }
	
	public partial class YqWeekService:BaseService<YqWeek>
    { 


	 static YqWeekService _instance;
        public static YqWeekService Instance
        {
            get
            {
                if (_instance == null)
                    return new YqWeekService();
                else
                    return _instance;
            }
        }

		public override void SetCurrentRepository()
        {
            CurrentRepository = _dbSession.YqWeekRepository;
        }  
    }
	
}