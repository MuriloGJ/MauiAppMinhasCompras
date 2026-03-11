using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
	ObservableCollection<Produto> lista = new ObservableCollection<Produto>();
	public ListaProduto()
	{
		InitializeComponent();
		lst_produtos.ItemsSource = lista;
	}
    protected async override void OnAppearing()
    {
		List<Produto> tmp = await App.Db.GetAll();
		tmp.ForEach(i => lista.Add(i));
    }
	private void ToolbarItem_Adicionar(object sender, EventArgs e)
	{
		try
		{
			Navigation.PushAsync(new Views.NovoProduto());

		}
		catch (Exception ex)
		{
			DisplayAlertAsync("Ops", ex.Message, "OK");
		}
	}

	
		private async void txt_search_TextChanged(object sender, TextChangedEventArgs e) 
	{
		string q = e.NewTextValue;

		lista.Clear();

        List<Produto> tmp = await App.Db.Search(q);
        tmp.ForEach(i => lista.Add(i));


    }

    

    private void ToolbarItem_somar(object sender, EventArgs e)
    {
		double soma = lista.Sum(i => i.Total);

		string msg = $"O total é {soma:C}";
		DisplayAlertAsync("Total dos Produtos", msg, "OK");

    }

   private async void SwipeItem_remover(object sender, EventArgs e)
    {
        try
        {
			SwipeItem item = (SwipeItem)sender;
			
                Produto p = (Produto)item.BindingContext;

                int id = p.Id;
            
            

            await App.Db.Delete(id);
            await DisplayAlertAsync("Sucesso!", "Registro Apagado", "OK");

        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }
}