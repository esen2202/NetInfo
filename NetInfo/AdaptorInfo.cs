using System.Net.NetworkInformation;

namespace NetInfo
{
    class AdaptorInfo
    {
        static AdaptorInfo adaptorInfo;
        private static object objLock;

        NetworkInterface[] adapters;

        private AdaptorInfo()
        {

        }

        public static AdaptorInfo CreateInstance()
        {
            if (adaptorInfo != null)
                lock (objLock)
                {
                    if (adaptorInfo != null)
                        adaptorInfo = new AdaptorInfo();
                }

            return adaptorInfo;
        }


    }
}
