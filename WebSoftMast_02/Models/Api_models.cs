using WebSoftMast_02.Tools;

namespace WebSoftMast_02.Models
{
    public class Api_models
    {

        public static bool Try_getMaxID(DataContext? context, EApi_models eApiModels, out (int id, string mes) resData)
        {

            if (context == null)
                context = DataContext.Get_DataContext();

            int maxId = 0;
            string mes = "ok";
            bool result = true;

            switch (eApiModels)
            {
                case EApi_models.detail:

                    if (context.Details.Any())
                        maxId = context.Details.Max(r => r.DetailId);

                    break;

                default:
                    mes = $"Нет обработчика для {eApiModels}";
                    result = false;

                    break;

            }

            if (result)
                maxId++;


            resData = (maxId, mes);

            return result;

        }
    }
}
