# Enterprise Resource Planning (ERP) & POS System

A comprehensive web-based ERP and Point of Sales (POS) solution designed to streamline business operations, from inventory and warehouse management to real-time financial reporting. This application is built for high performance and scalability using the latest .NET ecosystem.

## 🚀 Tech Stack

  * **Framework:** ASP.NET Core 8 (Web API / MVC)
  * **Database:** SQL Server / MySQL (Multi-provider support)
  * **ORM:** Entity Framework Core (Code First Approach)
  * **Authentication:** ASP.NET Core Identity / JWT
  * **Architecture:** Clean Architecture / N-Tier

## ✨ Key Features

  * 📦 **Inventory Management:** Detailed stock tracking with categories and unit management.
  * 🏬 **Multiple Warehouse:** Centralized control over stock across various locations.
  * 🛒 **Point of Sales (POS):** Fast and intuitive checkout interface for retail.
  * 📊 **Real-time Reporting:** Dynamic dashboards for sales, stock levels, and financial health.
  * 🏷️ **Barcode Support:** Integrated barcode scanning for rapid product lookup and transactions.
  * 🧾 **Fast Invoice Generation:** Instant PDF/Digital invoice creation upon transaction completion.
  * 🖨️ **Thermal Printing:** Direct integration with thermal printers for POS receipts.
  * 💸 **Expense Management:** Track and categorize business operational costs.
  * 🛒 **Purchase Management:** Handle Purchase Orders (PO), supplier management, and stock procurement.
  * 📈 **Sales Management:** Monitor the full sales cycle from quotation to payment.

## 🛠️ Installation & Setup

Ensure you have the [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) and your preferred database engine (SQL Server or MySQL) installed.

1.  **Clone the Repository**

    ```bash
    git clone https://github.com/jonathanzefanya/ERP.git
    cd ERP
    ```

2.  **Database Configuration**
    Update the connection string in `appsettings.json`:

    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=YOUR_SERVER;Database=ERP_DB;User Id=YOUR_USER;Password=YOUR_PASSWORD;"
    }
    ```

3.  **Apply Migrations**
    Run the following command in your terminal or Package Manager Console:

    ```bash
    dotnet ef database update
    ```

4.  **Run the Application**

    ```bash
    dotnet run
    ```

## 🤝 Contributing

Contributions are welcome\! If you'd like to improve the codebase or add new features:

1.  Fork the Project.
2.  Create your Feature Branch (`git checkout -b feature/AmazingFeature`).
3.  Commit your Changes (`git commit -m 'Add some AmazingFeature'`).
4.  Push to the Branch (`git push origin feature/AmazingFeature`).
5.  Open a Pull Request.

## 📄 License

Distributed under the **MIT License**. See `LICENSE` for more information.

-----

**Developed by [Jonathan Natannael Zefanya](https://www.google.com/search?q=https://github.com/jonathanzefanya)**