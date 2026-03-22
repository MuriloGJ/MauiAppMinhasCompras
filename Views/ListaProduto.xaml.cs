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

        lista.Clear();
        try
        {


            List<Produto> tmp = await App.Db.GetAll();
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
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
        try
        {
            string q = e.NewTextValue;

            lista.Clear();

            List<Produto> tmp = await App.Db.Search(q);
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
           await DisplayAlertAsync("Ops", ex.Message, "OK");
        }

    }



    private void ToolbarItem_somar(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);

        string msg = $"O total é {soma:C}";
        DisplayAlertAsync("Total dos Produtos", msg, "OK");

    }

    

    private async void MenuItem_remover(object sender, EventArgs e)
    {
        try
        {
            MenuItem item = sender as MenuItem;

            Produto p = item.BindingContext as Produto;

            bool confirma = await DisplayAlertAsync("Tem Certeza?", $"Remover {p.Descricao}", "Sim", "Não");

            if (confirma)
            {
                await App.Db.Delete(p.Id);
                lista.Remove(p);
                await DisplayAlertAsync("Sucesso!", "Registro Apagado", "OK");

            }
            else
            {
                await DisplayAlertAsync("Falha!", "Registro Mantido", "OK");
            }



        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try 
        {
            Produto p = e.SelectedItem as Produto;

            Navigation.PushAsync(new Views.EditarProduto
            { BindingContext = p, });


        }
        catch (Exception ex)
        {
            DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }
}
