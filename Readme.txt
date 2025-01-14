// Atualizar banco de dados LOCAL
dotnet ef dbcontext scaffold "Data Source=PEDRO;Initial Catalog=MEDIC_SCAN;Integrated Security=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer --context AppDbContext -o Models --force --no-onconfiguring

// Atualizar banco de dados PRODUÇÃO
dotnet ef dbcontext scaffold 'Server=db12541.public.databaseasp.net; Database=db12541; User Id=db12541; Password=9w!D#Pf6a2F-; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;' Microsoft.EntityFrameworkCore.SqlServer --context AppDbContext -o Models --force --no-onconfiguring
