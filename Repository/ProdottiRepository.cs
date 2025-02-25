﻿namespace Amazon;

public class IProdottiRepository : IProdottoService
{

    private ApplicationDbContext _applicationDbContext;

    public IProdottiRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public bool CancellaUnProdotto(int idDelProdotto)
    {
        return true;
    }
    public List<AddProdottoDtoInput> GetProdotti()
    {
        return new List<AddProdottoDtoInput>();
    }

    public InviaUnProdottoDto InviaUnProdotto()
    {
        return new InviaUnProdottoDto();
    }
    public List<AddProdottoDtoInput> GetListaDeiProdotti()
    {
        return new List<AddProdottoDtoInput>();
    }

    public List<AddProdottoDtoInput> AddProdotto()
    {
        return new List<AddProdottoDtoInput>();
    }

    public List<ProdottoDto> RestituiscimiUnProdotto()
    {
        return new List<ProdottoDto>();
    }

     public UpdateProdottoDtoInput ModificaProdotti()
    {
        return new UpdateProdottoDtoInput();
    }

    public ProdottoDto RestituiscimiUnSoloProdotto()
    {
        return new ProdottoDto();
    }

    public CreaProdottoInputDto CreaProdotto(CreaProdottoInputDto creaProdottoInputDto)
    {
        return new CreaProdottoInputDto();
    }

    public UpdateProdottoDtoInput UpdateProdotto(int IdDelProdotto, UpdateProdottoDtoInput prodotto)
    {
        return new UpdateProdottoDtoInput();
    }
    public ProdottoDtoInput GetProdottoDtoInput()
    {
        return new ProdottoDtoInput();
    }

    /* public AddProdottoDtoInput addProdotto(int idDelProdotto, AddProdotto addProdotto)
    {
        return new AddProdottoDtoInput();
    } */

    public AddProdottoDtoInput addProdotto(int idDelProdotto, AddProdottoDtoInput addProdotto)
    {
        return new AddProdottoDtoInput();
    }
    public List<ProdottoDtoInput> GetListProdotti()
    {
        return new List<ProdottoDtoInput>();
    }

    public int RestituiscimiLaQuantita(List<ProdottoDtoInput> listProdottoDto)
    {
        return listProdottoDto.Count();
    }

    public int ModificaLaQuantitàDeiProdotti()
    {
        throw new NotImplementedException();
    }

    public int InviaLaQuantitàDeiProdotti()
    {
        throw new NotImplementedException();
    }

    public ProdottoDtoInput GetProdotto(int numeroProdotto)
    {
        throw new NotImplementedException();
    }

    public int RestituiscimiLaQuantita()
    {
        throw new NotImplementedException();
    }

    public int ModificaLaQuantitàDeiProdotti(int quantita)
    {
        throw new NotImplementedException();
    }

    public object GetNumeroProdottiCreati()
    {
        throw new NotImplementedException();
    }
}

