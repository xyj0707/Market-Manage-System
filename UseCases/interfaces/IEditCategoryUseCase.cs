using CoreBusiness;

namespace UseCases.interfaces
{
    public interface IEditCategoryUseCase
    {
        void Execute(int categoryId, Category category);
    }
}