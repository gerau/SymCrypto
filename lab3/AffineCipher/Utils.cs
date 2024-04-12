using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AffineCipher
{
    internal class Utils
    {
        public static int Modulo(int a, int modulo)
        {
            return(a%modulo + modulo) % modulo;
        }
        public static int GCD(int a, int b)
        {
            while(b != 0 && a != 1)
            {
                int temp = a;
                a = b;
                b = temp % a;
            }
            return a;
        }

        public static int[] ExtentedGCD(int a, int b)
        {

            int u = 0;
            int v = 1;

            int old_u = 1;
            int old_v = 0;


            while (b != 0 && a != 1)
            {
                int q = a / b;

                int temp = u;
                u = old_u - u * q;
                old_u = temp;

                temp = v;
                v = old_v - v * q;
                old_v = temp;

                temp = a;
                a = b;
                b = temp % a;
            }
            return [old_u,old_v]; 
        }


        public static int[] SolveLinearComparision(int a, int b, int n)
        {
            a = Modulo(a, n);
            b = Modulo(b, n);   
            int gcd = GCD(a, n);
            if(gcd == 1)
            {
                return [Modulo(ExtentedGCD(a, n)[0] * b,n)];
            }
            else
            {
                if(b % gcd != 0)
                {
                    throw new Exception("Solution doesn't exist");
                }
                int[] result = new int[gcd];
             
                int new_a = a / gcd; 
                int new_b = b / gcd;
                int new_n = n / gcd;

                int solution = Modulo(ExtentedGCD(new_a, new_n)[0] * new_b,  new_n);

                for(int i = 0; i < gcd; i++)
                {
                    result[i] = solution + i*new_n;
                }
                return result;
            }
        }

    }
}
