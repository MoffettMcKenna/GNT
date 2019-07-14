using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNT.Endpoint.ASTP {

	/// <summary>
	/// The Advanced Simple Text Protocol uses a char by char encryption scheme, represented by this class.  The specific substituitions will be determined by the subclasses through the Template pattern.
	/// </summary>
	abstract class AbsASTPCypher{
		protected readonly char[] key = new char[] { '`', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '-', '=', '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '+', 'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p', '[', ']', '\\','Q','W','E','R','T','Y','U','I','O','P','{','}','|','a','s','d','f','g','h','j','k','l',';','\'','A','S','D','F','G','H','J','K','L',':','"','z','x','c','v','b','n','m',',','.','/','Z','X','C','V','B','N','M','<','>','?',' ','\t','\n','\r'};

		/// <summary>
		/// Encrypts a string.
		/// </summary>
		/// <param name="data">The raw string.</param>
		/// <returns>The encrypted data.</returns>
		public string Encrpyt(string data) {
			//create the container for the encrypted string
			List<char> retval = new List<char>();

			//let the subclass handle the encryption (Template Method!)
			foreach(char c in data.ToCharArray()) retval.Add(encryptChar(c));

			//return the encrypted data
			return new String(retval.ToArray());
		}

		/// <summary>
		/// Decrypts a string into the actual value.
		/// </summary>
		/// <param name="data">The encrypted data.</param>
		/// <returns>The decrypted string.</returns>
		public string Decrypt(string data) {
			//create the container for the decrypted string
			List<char> retval = new List<char>();

			//let the subclass handle the decryption (Template Method!)
			foreach (char c in data.ToCharArray()) retval.Add(decryptChar(c));

			//return the decrypted data
			return new String(retval.ToArray());
		}

		/// <summary>
		/// Implemented by the child classes to perform the decryption option on the individual characters.
		/// </summary>
		/// <param name="c">The character to decrypt.</param>
		/// <returns>The decrypted value.</returns>
		protected abstract char decryptChar(char c);
		/// <summary>
		/// Implemented by the child classes to perform the encryption option on the individual characters.
		/// </summary>
		/// <param name="c">The character to encrypt.</param>
		/// <returns>The encrypted value.</returns>
		protected abstract char encryptChar(char c);
		/// <summary>
		/// Create a copy of this cypher instance.
		/// </summary>
		/// <returns>A copy of the cypher instance, including the current encryption key.</returns>
		public abstract AbsASTPCypher Clone();
	}

	class LeftShiftCypher : AbsASTPCypher {
		private int shift;

		public LeftShiftCypher() {
			shift = 1;
		}

		#region AbsASTPCypher
		/// <summary>
		/// Implemented by the child classes to perform the decryption option on the individual characters.
		/// </summary>
		/// <param name="c">The character to decrypt.</param>
		/// <returns>The decrypted value.</returns>
		protected override char decryptChar(char c) {
			char r = ' ';
			int dex = Array.IndexOf(key, c);
			if (dex >= 0) r = key[dex + shift];
			shift++;
			return r;
		}

		/// <summary>
		/// Implemented by the child classes to perform the encryption option on the individual characters.
		/// </summary>
		/// <param name="c">The character to encrypt.</param>
		/// <returns>The encrypted value.</returns>
		protected override char encryptChar(char c) {
			char r = ' ';
			int dex = Array.IndexOf(key, c);
			if (dex >= 0) r = key[dex - shift];
			shift++;
			return r;
		}

		/// <summary>
		/// Create a copy of this cypher instance.
		/// </summary>
		/// <returns>A copy of the cypher instance, including the current encryption key.</returns>
		public override AbsASTPCypher Clone() {
			return (this.MemberwiseClone() as LeftShiftCypher);
		}
		#endregion

	}

	/// <summary>
	/// 
	/// </summary>
	class RightShiftCypher : AbsASTPCypher {
		private int shift;

		public RightShiftCypher() {
			shift = 1;
		}

		#region AbsASTPCypher
		/// <summary>
		/// Implemented by the child classes to perform the decryption option on the individual characters.
		/// </summary>
		/// <param name="c">The character to decrypt.</param>
		/// <returns>The decrypted value.</returns>
		protected override char decryptChar(char c) {
			char r = ' ';
			int dex = Array.IndexOf(key, c);
			if (dex >= 0) r = key[dex - shift];
			shift++;
			return r;
		}

		/// <summary>
		/// Implemented by the child classes to perform the encryption option on the individual characters.
		/// </summary>
		/// <param name="c">The character to encrypt.</param>
		/// <returns>The encrypted value.</returns>
		protected override char encryptChar(char c) {
			char r = ' ';
			int dex = Array.IndexOf(key, c);
			if (dex >= 0) r = key[dex + shift];
			shift++;
			return r;
		}

		/// <summary>
		/// Create a copy of this cypher instance.
		/// </summary>
		/// <returns>A copy of the cypher instance, including the current encryption key.</returns>
		public override AbsASTPCypher Clone() {
			return (this.MemberwiseClone() as LeftShiftCypher);
		}
		#endregion
	}
}
