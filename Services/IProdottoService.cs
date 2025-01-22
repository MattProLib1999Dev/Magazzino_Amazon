using Amazon;

public interface IProdottoService
{
    List<AddProdottoDtoInput> GetProdotti();
    ProdottoDtoInput GetProdotto(int numeroProdotto);
    bool CancellaUnProdotto(int idDelProdotto);
    InviaUnProdottoDto InviaUnProdotto();
    CreaProdottoDto CreaProdotto(CreaProdottoInputDto creaProdottoInputDto);
    UpdateProdottoDtoInput UpdateProdotto(int IdDelProdotto, UpdateProdottoDtoInput prodotto);
    AddProdottoDtoInput addProdotto(int idDelProdotto, AddProdottoDtoInput addProdotto);
    List<ProdottoDtoInput> GetListProdotti();
    int RestituiscimiLaQuantita(List<ProdottoDtoInput> listProdottoDto);
    int ModificaLaQuantitàDeiProdotti();
    int InviaLaQuantitàDeiProdotti();

}

public class ProdottoService : IProdottoService
{
    public List<CreaProdottoInputDto> RestituiscimiUnaListaDiProdotti()
    {
        return new List<CreaProdottoInputDto>();
    }
    public bool CancellaUnProdotto(int idDelProdotto)
    {
        return true;
    }
    public InviaUnProdottoDto InviaUnProdotto()
    {
        return new InviaUnProdottoDto();
    }
    /* public List<AddProdottoDtoInput> GetListProdotti()
    {
        return new List<AddProdottoDtoInput>();
    } */
    public ProdottoDto RestituiscimiUnSoloProdotto()
    {
        return new ProdottoDto();
    }

    public CreaProdottoDto CreaProdotto(CreaProdottoInputDto creaProdottoInputDto)
    {
        return new CreaProdottoDto();
    }
    public UpdateProdottoDtoInput UpdateProdotto(int IdDelProdotto, UpdateProdottoDtoInput prodotto)
    {
        return new UpdateProdottoDtoInput();
    }
    public ProdottoDtoInput GetProdottoDtoInput()
    {
        return new ProdottoDtoInput();
    }

    public ProdottoDtoInput GetProdotto(int numeroProdotto)
    {
        return new ProdottoDtoInput();
    }

    public AddProdottoDto addProdotto(int idDelProdotto, AddProdotto addProdotto)
    {
        return new AddProdottoDto();
    }

    public AddProdottoDtoInput addProdotto(int idDelProdotto, AddProdottoDtoInput addProdotto)
    {
        return new AddProdottoDtoInput();
    }
    public List<AddProdottoDtoInput> GetListaProdotti()
    {
        return new List<AddProdottoDtoInput>();
    }

    public List<ProdottoDtoInput> GetListProdotti() 
    {
        return new List<ProdottoDtoInput>();
    }

    public List<AddProdottoDtoInput> GetProdotti()
    {
        throw new NotImplementedException();
    }

    List<ProdottoDtoInput> IProdottoService.GetListProdotti()
    {
        throw new NotImplementedException();
    }
    
    public int RestituiscimiLaQuantita(List<ProdottoDtoInput> listProdottoDto)
    {
        return listProdottoDto.Count();
    }

    int IProdottoService.ModificaLaQuantitàDeiProdotti()
    {
        throw new NotImplementedException();
    }

    int IProdottoService.InviaLaQuantitàDeiProdotti()
    {
        throw new NotImplementedException();
    }

   

    /* public List<CreaProdottoInputDto> GetListaDeiProdotti()
    {
        throw new 
    } */
}