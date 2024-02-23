using CoreBusiness;

namespace UseCases.interfaces
{
    public interface IViewSelectedCategoryUseCase
    {
        Category? Execute(int categoryId);
    }
}