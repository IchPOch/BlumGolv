// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Text;

class RC4 {
    byte[] S = new byte[256];
    int x = 0;
    int y = 0;
    private void init(byte[] key)
    {
        int keyLength = key.Length;

        for (int i = 0; i < 256; i++)
        {
            S[i] = (byte)i;
        }

        int j = 0;
        byte buf;
        for (int i = 0; i < 256; i++)
        {
            j = (j + S[i] + key[i % keyLength]) % 256;
            buf = S[i];
            S[i] = S[j];
            S[j] = buf;

        }
    }
    public RC4(byte[] key)
    {
        init(key);
    }
    private byte keyItem()
    {
        byte buf;
        x = (x + 1) % 256;
        y = (y + S[x]) % 256;

        buf = S[x];
        S[x] = S[y];
        S[y] = buf;

        return S[(S[x] + S[y]) % 256];
    }
    public byte[] Encode(byte[] dataB, int size)
    {
        byte[] data = dataB.Take(size).ToArray();

        byte[] cipher = new byte[data.Length];

        for (int m = 0; m < data.Length; m++)
        {
            cipher[m] = (byte)(data[m] ^ keyItem());
        }

        return cipher;
    }
    public byte[] Decode(byte[] dataB, int size)
    {
        return Encode(dataB, size);
    }
};
class Program
{
    static void Main(string[] args)
    {
        byte[] key = ASCIIEncoding.ASCII.GetBytes("Key");

        RC4 encoder = new RC4(key);
        string testString = "Plaintext";
        byte[] testBytes = ASCIIEncoding.ASCII.GetBytes(testString);
        byte[] result = encoder.Encode(testBytes, testBytes.Length);
        Console.WriteLine("test = " + testBytes.ToString());
        Console.WriteLine("nashe = " + result.ToString());
        RC4 decoder = new RC4(key);
        byte[] decryptedBytes = decoder.Decode(result, result.Length);
        string decryptedString = ASCIIEncoding.ASCII.GetString(decryptedBytes);
        Console.WriteLine("testE = " + decryptedString.ToString());
        Console.WriteLine("nasheE = " + decryptedBytes.ToString());
    }
}