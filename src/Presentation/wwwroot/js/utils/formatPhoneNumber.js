function formatPhone(input) {
    var value = input.value.replace(/\D/g, ''); // Remove tudo que não for dígito
    value = value.replace(/^(\d{2})(\d)/g, '($1) $2'); // Coloca o DDD entre parênteses
    value = value.replace(/(\d{5})(\d{4})$/, '$1-$2'); // Adiciona o traço após os 5 dígitos
    input.value = value;
}