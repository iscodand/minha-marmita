document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("customerForm").addEventListener("submit", async function (event) {
        event.preventDefault();
        const url = "/clientes";

        const customerName = document.getElementById("customerName").value;
        const customerPhoneNumber = document.getElementById("customerPhoneNumber").value;
        const customerFormError = document.getElementById("customerFormErrorMessage");

        const requestBody = {
            name: customerName,
            phone: customerPhoneNumber
        };

        try {
            const response = await fetch(url, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(requestBody)
            });

            if (!response.ok) {
                const errorData = await response.json();
                customerFormError.innerHTML = errorData.errors;
                document.getElementById("mealFormError").classList.remove("hidden");
                throw new Error(`Erro: ${response.status}`);
            }

            const data = await response.json();

            document.getElementById("customerFormModal").classList.add("hidden");

            let newCustomerOption = new Option(`${data.data.id} - ${data.data.name}`, data.data.id, true, true);
            $('#customer-dynamic-select').append(newCustomerOption).trigger('change');

        } catch (error) {
            console.error("Erro na requisição:", error);
        }
    });
});