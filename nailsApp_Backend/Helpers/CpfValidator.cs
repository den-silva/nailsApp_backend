namespace nailsApp_Backend.Helpers
{
    public static class CpfValidator
    {
        public static bool ValidarCpf(string cpf)
        {
            // Remove caracteres não numéricos
            cpf = new string(cpf.Where(char.IsDigit).ToArray());
            
            if (cpf.Length != 11)
                return false;
            
            // Verifica se todos os dígitos são iguais
            if (cpf.Distinct().Count() == 1)
                return false;
            
            // Calcula primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * (10 - i);
            
            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;
            
            if (int.Parse(cpf[9].ToString()) != digito1)
                return false;
            
            // Calcula segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * (11 - i);
            
            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;
            
            return int.Parse(cpf[10].ToString()) == digito2;
        }
        
        public static string FormatarCpf(string cpf)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray());
            
            if (cpf.Length != 11)
                return cpf;
            
            return Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
        }
    }
}