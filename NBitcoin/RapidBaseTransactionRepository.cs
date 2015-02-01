﻿#if !NOHTTPCLIENT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NBitcoin
{
	public class RapidBaseTransactionRepository : ITransactionRepository
	{
		private readonly Uri _BaseUri;
		public Uri BaseUri
		{
			get
			{
				return _BaseUri;
			}
		}
		public RapidBaseTransactionRepository(Uri baseUri)
			: this(baseUri.AbsoluteUri)
		{

		}

		public RapidBaseTransactionRepository(string baseUri)
		{
			if(!baseUri.EndsWith("/"))
				baseUri += "/";
			_BaseUri = new Uri(baseUri, UriKind.Absolute);
		}



		#region ITransactionRepository Members
		
		public async Task<Transaction> GetAsync(uint256 txId)
		{
			using(HttpClient client = new HttpClient())
			{
				var tx = await client.GetAsync(BaseUri.AbsoluteUri + "transactions/" + txId + "?format=raw").ConfigureAwait(false);
				if(tx.StatusCode == System.Net.HttpStatusCode.NotFound)
					return null;
				tx.EnsureSuccessStatusCode();
				var bytes = await tx.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
				return new Transaction(bytes);
			}
		}

		public Task PutAsync(uint256 txId, Transaction tx)
		{
			return Task.FromResult(false);
		}

		#endregion
	}
}
#endif