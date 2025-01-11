using Amazon.Models.Request;
using Amazon.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Amazon
{
    public interface IProdottoInterfaces
    {
        List<ProdottoEntity> GetProdotti();
        List<IEnumerable<ProdottoEntity>> GetListadeiProdotti();
        ActionResult<List<ProdottoEntity>> GetListaDeiProdotti();

        public Task<UserInfoHandlerResponse> UserInfo(UserInfoDALRequest request);
    }

}