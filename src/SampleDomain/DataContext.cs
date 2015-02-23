 
 
namespace SampleDomain
{
    using NHibernate.DataContext;

    public class DataContext : DbContextBase<DataContext>
    {     
        
        
        #region SampleDomain.ACL      
  
        private DbRepoQueryContext<SampleDomain.ACL.User> m_ACL_User = null;
        public DbRepoQueryContext<SampleDomain.ACL.User> ACL_User 
        { 
            get
            {
                if (m_ACL_User == null)
                {
                    m_ACL_User = CreateRepoInternalSet<SampleDomain.ACL.User>(false);
                }

                return m_ACL_User;
            }
        }        
        #endregion

        
        #region SampleDomain.Model      
  
        private DbRepoQueryContext<SampleDomain.Model.Message> m_Model_Message = null;
        public DbRepoQueryContext<SampleDomain.Model.Message> Model_Message 
        { 
            get
            {
                if (m_Model_Message == null)
                {
                    m_Model_Message = CreateRepoInternalSet<SampleDomain.Model.Message>(false);
                }

                return m_Model_Message;
            }
        }        
        #endregion

            
    }
}

