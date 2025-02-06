document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("mealForm").addEventListener("submit", async function (event) {
        event.preventDefault();
        const url = "/sabores";

        const mealName = document.getElementById("mealName").value;
        const mealAccompaniments = document.getElementById("mealAccompaniments").value;
        const mealFormError = document.getElementById("mealFormErrorMessage");

        const requestBody = {
            description: mealName,
            accompaniments: mealAccompaniments
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
                mealFormError.innerHTML = errorData.errors;
                document.getElementById("mealFormError").classList.remove("hidden");
                throw new Error(`Erro: ${response.status}`);
            }

            const data = await response.json();

            document.getElementById("mealFormModal").classList.add("hidden");

            let newMealOption = new Option(`${data.data.id} - ${data.data.description}`, data.data.id, true, true);
            $('#dynamic-select').append(newMealOption).trigger('change');

        } catch (error) {
            console.error("Erro na requisição:", error);
        }
    });
});