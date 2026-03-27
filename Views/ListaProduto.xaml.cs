using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();
    ObservableCollection<TotalCategoria> listac = new ObservableCollection<TotalCategoria>();

    public ListaProduto()
    {
        InitializeComponent();
        lst_produtos.ItemsSource = lista;
    
    }
    protected async override void OnAppearing()
    {


        try
        {
            lista.Clear();

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

            txt_categoria.IsEnabled = string.IsNullOrWhiteSpace(q);


            if (!string.IsNullOrWhiteSpace(q))
                txt_categoria.Text = "";

            lst_produtos.IsRefreshing = true;

            lista.Clear();

            List<Produto> tmp = await App.Db.Search_desc(q);
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }

    }
    private async void txt_categoria_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string c = e.NewTextValue;

            txt_search.IsEnabled = string.IsNullOrWhiteSpace(c);


            if (!string.IsNullOrWhiteSpace(c))
                txt_search.Text = "";

            lst_produtos.IsRefreshing = true;

            lista.Clear();

            List<Produto> tmp = await App.Db.Search_cat(c);
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
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
    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        try
        {
            lista.Clear();

            List<Produto> tmp = await App.Db.GetAll();
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }
    private async void ToolbarItem_categoria(object sender, EventArgs e)
    {
        try
        {



            List<TotalCategoria> tmp = await App.Db.TotalPorCategorias();

            tmp.ForEach(i => listac.Add(i));

            string msg = string.Join("\n", tmp.Select(i =>
     $"{i.Categoria}: {i.Totalc:C}"
 ));


            await DisplayAlertAsync("Totais por Categoria", msg, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", ex.Message, "OK");
        }
        /*finally
        {
            lst_produtos.IsRefreshing = false;
        }*/

        /* var lista = await App.Db.TotalPorCategorias();

         string msg = "";

         foreach (var item in lista)
         {
             msg += $"{item.Categoria}: {item.Totalc:C}\n";
    }

        await DisplayAlertAsync("Totais por Categoria", msg, "OK");
    }
    catch (Exception ex)
    {
        await DisplayAlertAsync("Erro", ex.Message, "OK");
    }
}*/
    }
}
    