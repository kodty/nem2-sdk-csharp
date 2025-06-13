using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit_Tests.Model
{
    public class QueryModelTests
    {
        [Test]
        public void VerifyParameterCompatability()
        {
            var queryModelArray = new QueryModel[] { 
                                    new QueryModel(QueryModel.DefineRequest.GetRecieptsAddressResolutionStatements),
                                    new QueryModel(QueryModel.DefineRequest.GetRecieptsMosaicResolutionStatements),
                                    new QueryModel(QueryModel.DefineRequest.SearchAccountRestrictions),
                                    new QueryModel(QueryModel.DefineRequest.SearchAccounts),
                                    new QueryModel(QueryModel.DefineRequest.SearchBlocks),
                                    new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions),
                                    new QueryModel(QueryModel.DefineRequest.SearchHashLockTransactions),
                                    new QueryModel(QueryModel.DefineRequest.SearchMetaDataEntries),
                                    new QueryModel(QueryModel.DefineRequest.SearchMosaicRestrictions),
                                    new QueryModel(QueryModel.DefineRequest.SearchMosaics),
                                    new QueryModel(QueryModel.DefineRequest.SearchNamespaces),
                                    new QueryModel(QueryModel.DefineRequest.SearchPartialTransactions),
                                    new QueryModel(QueryModel.DefineRequest.SearchSecretLockTransactions),
                                    new QueryModel(QueryModel.DefineRequest.SearchTransactionStatements),
                                    new QueryModel(QueryModel.DefineRequest.SearchUnconfirmedTransactions)};

            queryModelArray[0].SetParam(QueryModel.DefinedParams.height, 78882);
            queryModelArray[0].SetParam(QueryModel.DefinedParams.pageSize, 10);
            queryModelArray[0].SetParam(QueryModel.DefinedParams.pageNumber, 1);
           // queryModelArray[0].SetParam(QueryModel.DefinedParams.offset, "680AB32C566B3AF4591078E9");
            queryModelArray[0].SetParam(QueryModel.DefinedParams.order, QueryModel.Order.Asc);

            queryModelArray[1].SetParam(QueryModel.DefinedParams.height, 1126);
            queryModelArray[1].SetParam(QueryModel.DefinedParams.pageSize, 10);
            queryModelArray[1].SetParam(QueryModel.DefinedParams.pageNumber, 1);
            // queryModelArray[1].SetParam(QueryModel.DefinedParams.offset, "680AB32C566B3AF4591078E9");
            queryModelArray[1].SetParam(QueryModel.DefinedParams.order, QueryModel.Order.Asc);

            queryModelArray[2].SetParam(QueryModel.DefinedParams.address, "6808E6B1F56C7EA1409924467213B9CA7EDBD4ED05F0DCD3");
            queryModelArray[2].SetParam(QueryModel.DefinedParams.pageSize, 10);
            queryModelArray[2].SetParam(QueryModel.DefinedParams.pageNumber, 1);
            // queryModelArray[2].SetParam(QueryModel.DefinedParams.offset, "680AB32C566B3AF4591078E9");
            queryModelArray[2].SetParam(QueryModel.DefinedParams.order, QueryModel.Order.Asc);

            queryModelArray[3].SetParam(QueryModel.DefinedParams.pageSize, 10);
            queryModelArray[3].SetParam(QueryModel.DefinedParams.pageNumber, 20);
            queryModelArray[3].SetParam(QueryModel.DefinedParams.offset, "680AB31563DB1818A9725C28");
            queryModelArray[3].SetParam(QueryModel.DefinedParams.order, QueryModel.Order.Asc);
            queryModelArray[3].SetParam(QueryModel.DefinedParams.orderBy, QueryModel.OrderBy.Id);
            queryModelArray[3].SetParam(QueryModel.DefinedParams.mosaicId, "6BED913FA20223F8");

            queryModelArray[4].SetParam(QueryModel.DefinedParams.signerPublicKey, "BE0B4CF546B7B4F4BBFCFF9F574FDA527C07A53D3FC76F8BB7DB746F8E8E0A9F");
            queryModelArray[4].SetParam(QueryModel.DefinedParams.beneficiaryAddress, "NASYMBOLLK6FSL7GSEMQEAWN7VW55ZSZU25TBOA");
            queryModelArray[4].SetParam(QueryModel.DefinedParams.pageSize, 10);
            queryModelArray[4].SetParam(QueryModel.DefinedParams.pageNumber, 1);            
            queryModelArray[4].SetParam(QueryModel.DefinedParams.order, QueryModel.Order.Asc);
            queryModelArray[4].SetParam(QueryModel.DefinedParams.orderBy, QueryModel.OrderBy.Id);
            
            //queryModelArray[5].SetParam(QueryModel.DefinedParams.address, ""); // cant be used with recipient address or signer public key
            queryModelArray[5].SetParam(QueryModel.DefinedParams.recipientAddress, "68F35B3CA84DB724B948E7A6B2DAB065E2FD4D51139BA3A6"); // api has a bullshit typo that shouldn't be there.
            queryModelArray[5].SetParam(QueryModel.DefinedParams.signerPublicKey, "90E1D2A533D6715235CB49CFBD69EE0A69B8F89C8FD74C02546E5A9E54498F80");
            queryModelArray[5].SetParam(QueryModel.DefinedParams.height, 79760);
            queryModelArray[5].SetParam(QueryModel.DefinedParams.fromHeight, 79760);
            queryModelArray[5].SetParam(QueryModel.DefinedParams.toHeight, 79760);
            queryModelArray[5].SetParam(QueryModel.DefinedParams.pageNumber, 1);
            queryModelArray[5].SetParam(QueryModel.DefinedParams.pageSize, 10);

            queryModelArray[6].SetParam(QueryModel.DefinedParams.address, "NASYMBOLLK6FSL7GSEMQEAWN7VW55ZSZU25TBOA");
            queryModelArray[6].SetParam(QueryModel.DefinedParams.pageSize, 10);
            queryModelArray[6].SetParam(QueryModel.DefinedParams.pageNumber, 1);
            queryModelArray[6].SetParam(QueryModel.DefinedParams.order, QueryModel.Order.Asc);

            queryModelArray[7].SetParam(QueryModel.DefinedParams.sourceAddress, "68D59ED4096C5CF366F65E554C7BC0FABEA0E60D4B28FB4A");
            queryModelArray[7].SetParam(QueryModel.DefinedParams.targetAddress, "68D59ED4096C5CF366F65E554C7BC0FABEA0E60D4B28FB4A");
            queryModelArray[7].SetParam(QueryModel.DefinedParams.scopedMetadataKey, "501EA9C5BF005AE1");
            queryModelArray[7].SetParam(QueryModel.DefinedParams.targetId, "0A795B69698C01D6");
            queryModelArray[7].SetParam(QueryModel.DefinedParams.metadataType, 1);

            queryModelArray[8].SetParam(QueryModel.DefinedParams.mosaicId, "613E6D0FC11F4530");
            queryModelArray[8].SetParam(QueryModel.DefinedParams.entryType, 0);
            queryModelArray[8].SetParam(QueryModel.DefinedParams.targetAddress, "NAT6KCGFXOBBVK2FIFKH2AYHE6G6G6EENK4CJVY");
            queryModelArray[8].SetParam(QueryModel.DefinedParams.pageSize, 1);
            queryModelArray[8].SetParam(QueryModel.DefinedParams.pageNumber, 1);
            queryModelArray[8].SetParam(QueryModel.DefinedParams.order, QueryModel.Order.Asc);

            queryModelArray[9].SetParam(QueryModel.DefinedParams.ownerAddress, "NASYMBOLLK6FSL7GSEMQEAWN7VW55ZSZU25TBOA");
            queryModelArray[9].SetParam(QueryModel.DefinedParams.pageSize, 1);
            queryModelArray[9].SetParam(QueryModel.DefinedParams.pageNumber, 1);
            //queryModelArray[8].SetParam(QueryModel.DefinedParams.offset, "");
            queryModelArray[9].SetParam(QueryModel.DefinedParams.order, QueryModel.Order.Asc);

            Assert.That(queryModelArray[0].ReturnPathParams(), Is.EqualTo("height=78882&pageSize=10&pageNumber=1&order=Asc")); // see why receipt height at 923
            Assert.That(queryModelArray[1].ReturnPathParams(), Is.EqualTo("height=1126&pageSize=10&pageNumber=1&order=Asc"));
            Assert.That(queryModelArray[2].ReturnPathParams(), Is.EqualTo("address=6808E6B1F56C7EA1409924467213B9CA7EDBD4ED05F0DCD3&pageSize=10&pageNumber=1&order=Asc"));
            Assert.That(queryModelArray[3].ReturnPathParams(), Is.EqualTo("pageSize=10&pageNumber=20&offset=680AB31563DB1818A9725C28&order=Asc&orderBy=Id&mosaicId=6BED913FA20223F8"));
            Assert.That(queryModelArray[4].ReturnPathParams(), Is.EqualTo("signerPublicKey=BE0B4CF546B7B4F4BBFCFF9F574FDA527C07A53D3FC76F8BB7DB746F8E8E0A9F&beneficiaryAddress=NASYMBOLLK6FSL7GSEMQEAWN7VW55ZSZU25TBOA&pageSize=10&pageNumber=1&order=Asc&orderBy=Id"));
            Assert.That(queryModelArray[5].ReturnPathParams(), Is.EqualTo("recipientAddress=68F35B3CA84DB724B948E7A6B2DAB065E2FD4D51139BA3A6&signerPublicKey=90E1D2A533D6715235CB49CFBD69EE0A69B8F89C8FD74C02546E5A9E54498F80&height=79760&fromHeight=79760&toHeight=79760&pageNumber=1&pageSize=10"));
            Assert.That(queryModelArray[6].ReturnPathParams(), Is.EqualTo("address=NASYMBOLLK6FSL7GSEMQEAWN7VW55ZSZU25TBOA&pageSize=10&pageNumber=1&order=Asc")); // see why receipt height at 923
            Assert.That(queryModelArray[7].ReturnPathParams(), Is.EqualTo("sourceAddress=68D59ED4096C5CF366F65E554C7BC0FABEA0E60D4B28FB4A&targetAddress=68D59ED4096C5CF366F65E554C7BC0FABEA0E60D4B28FB4A&scopedMetadataKey=501EA9C5BF005AE1&targetId=0A795B69698C01D6&metadataType=1"));
            Assert.That(queryModelArray[8].ReturnPathParams(), Is.EqualTo("mosaicId=613E6D0FC11F4530&entryType=0&targetAddress=NAT6KCGFXOBBVK2FIFKH2AYHE6G6G6EENK4CJVY&pageSize=1&pageNumber=1&order=Asc"));
            Assert.That(queryModelArray[9].ReturnPathParams(), Is.EqualTo("ownerAddress=NASYMBOLLK6FSL7GSEMQEAWN7VW55ZSZU25TBOA&pageSize=1&pageNumber=1&order=Asc"));
        }
    }
}
                                    