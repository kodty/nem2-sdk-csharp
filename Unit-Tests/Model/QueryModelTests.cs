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
            //queryModelArray[4].SetParam(QueryModel.DefinedParams.offset, "<id>"); // validate against multi block response. funcationality appears to return blocks after offset.
            queryModelArray[4].SetParam(QueryModel.DefinedParams.order, QueryModel.Order.Asc);
            queryModelArray[4].SetParam(QueryModel.DefinedParams.orderBy, QueryModel.OrderBy.Id);


            //queryModelArray[2].SetParam(QueryModel.DefinedParams.sourceAddress, 10);
            //queryModelArray[2].SetParam(QueryModel.DefinedParams.targetAddress, 20);
            //queryModelArray[2].SetParam(QueryModel.DefinedParams.scopedMetadataKey, 10);
            //queryModelArray[2].SetParam(QueryModel.DefinedParams.targetId, 20);
            //queryModelArray[2].SetParam(QueryModel.DefinedParams.metadataType, 0);
            //queryModelArray[2].SetParam(QueryModel.DefinedParams.pageSize, 0);
            //queryModelArray[2].SetParam(QueryModel.DefinedParams.pageNumber, 0);
            //queryModelArray[2].SetParam(QueryModel.DefinedParams.offset, 0);
            //queryModelArray[2].SetParam(QueryModel.DefinedParams.order, QueryModel.Order.Asc);

             //pageSize=10&pageNumber=20&offset=680AB31563DB1818A9725C28&order=Asc&orderBy=Id&mosaicId=680AB31563DB1818A9725C28
            
            
            Assert.That(queryModelArray[3].ReturnPathParams(), Is.EqualTo("pageSize=10&pageNumber=20&offset=680AB31563DB1818A9725C28&order=Asc&orderBy=Id&mosaicId=6BED913FA20223F8"));

            Assert.That(queryModelArray[4].ReturnPathParams(), Is.EqualTo("signerPublicKey=BE0B4CF546B7B4F4BBFCFF9F574FDA527C07A53D3FC76F8BB7DB746F8E8E0A9F&beneficiaryAddress=NASYMBOLLK6FSL7GSEMQEAWN7VW55ZSZU25TBOA&pageSize=10&pageNumber=1&order=Asc&orderBy=Id"));

        }
    }
}
                                    