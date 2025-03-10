using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class EmailContentBuilder
    {
        public static string BuildNotiMail(string name, DateTime? appointmentDate)
        {
            string formattedDate = appointmentDate?.ToString("dd/MM/yyyy") ?? "ngày không xác định";
            return $@"<!DOCTYPE html>
    <html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Email Notification</title>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #f4f4f4;
                margin: 0;
                padding: 20px;
                justify-content: center;
                align-items: center;
                flex-direction: column;
            }}
            .container {{
                background-color: #0099cf;
                border-radius: 5px;
                padding: 20px;
                box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                width: 80%;
                max-width: 600px;
                flex-direction: column; 
                align-items: center; 
                text-align: center;
            }}
            h1 {{
                color: #333;
            }}
            p {{
                color: #555;
            }}
            .footer {{
                margin-top: 20px;
                font-size: 12px;
                color: #888;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <img src='https://cdn.youmed.vn/tin-tuc/wp-content/uploads/2020/09/su-hinh-thanh-va-phat-trien-thai-nhi-trong-tam-ca-nguye.jpg' width='600'>
            <h1>Chào {name}</h1>
            <p>Đã gần đến ngày hẹn khám bác sĩ của mẹ và bé: {formattedDate}.
            <br>Nhớ chuẩn bị đầy đủ các giấy tờ, hồ sơ bệnh án cần thiết để có một ngày khám bệnh suôn sẻ nhé! 
            <br>Một số giấy tờ hồ sơ quan trọng như sau: 
                <br><b>- Sổ khám bệnh. 
                <br>- Căn cước công dân.
                <br>- Thẻ bảo hiểm y tế.
                <br>- Giấy hẹn tái khám của bác sĩ. 
                <br>- Hình ảnh và xét nghiệm.
                <br>- Hướng dẫn điều trị.
                <br>- Đơn thuốc đang sử dụng.</b>
            <br>Bạn có thể chia sẻ biểu đồ số liệu sức khỏe của mình và thông báo cho bác sĩ về bất kỳ triệu chứng hoặc chỉ số bất thường nào bạn đang gặp phải.
            <br>Hệ thống của chúng tôi rất vui khi được phục vụ và đồng hành cùng bạn ở giai đoạn vàng này.</p>
            <div class='footer'>
                <p>Chúc mẹ và bé có thật nhiều sức khỏe ^^</p>
            </div>
        </div>
    </body>
    </html>";
        }
    }
}
