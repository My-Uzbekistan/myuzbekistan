using ActualLab.Fusion;
using ActualLab.Fusion.EntityFramework;
using ClosedXML.Excel;
using myuzbekistan.Services;
using myuzbekistan.Shared;

namespace Services.Features.User
{
    public class UserService(IServiceProvider services) : DbServiceBase<ApplicationDbContext>(services), IUserService
    {


        public async virtual Task<List<ApplicationUser>> Get(long Id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async virtual Task<TableResponse<ApplicationUser>> GetAll(TableOptions options, CancellationToken cancellationToken)
        {
            await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
            var count = dbContext.Users.Count();
            var items = dbContext.Users.Paginate(options).ToList();
            return new TableResponse<ApplicationUser>() { Items = items, TotalItems = count };
        }



        public async virtual Task<string> UserToExcel(UserToExcelCommand command,
            CancellationToken cancellationToken = default)
        {
            command.Options.PageSize = 1000000;
            var users = await GetAll(command.Options, cancellationToken);


            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("users");
            var currentRow = 1;
            var headers = new[]{ "Id", "Email", "CreatedAt" };

            for (int col = 2; col < headers.Length + 2; col++)
            {
                worksheet.Cell(currentRow, col).Value = headers[col - 2];
                worksheet.Cell(currentRow, col).Style.Font.Bold = true;
                worksheet.Cell(currentRow, col).Style.Fill.BackgroundColor = XLColor.LightBlue;
                worksheet.Cell(currentRow, col).Style.Font.FontColor = XLColor.Black;
                worksheet.Cell(currentRow, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                worksheet.Cell(currentRow, col).Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Cell(currentRow, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }

            foreach (var user in users.Items.ToList())
            {
                currentRow++;
                worksheet.Cell(currentRow, 2).Value = user.Id;
                worksheet.Cell(currentRow, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;
                worksheet.Cell(currentRow, 2).Style.Font.FontColor = XLColor.Black;
                worksheet.Cell(currentRow, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(currentRow, 2).Style.Border.OutsideBorderColor = XLColor.Black;

                worksheet.Cell(currentRow, 3).Value = user.Email;
                worksheet.Cell(currentRow, 4).Value = user.CreatedAt;

            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return Convert.ToBase64String(stream.ToArray());
        }
    }
}
