using Xunit;

namespace Ecommerce.Architecture.Tests;

public sealed class ModuleBoundaryTests
{
    [Fact]
    public void Catalog_module_only_references_inventory_public_contracts()
    {
        var catalogRoot = Path.Combine(RepoRoot(), "src", "backend", "src", "Ecommerce.Modules.Catalog");
        var catalogFiles = Directory.EnumerateFiles(catalogRoot, "*.cs", SearchOption.AllDirectories);

        foreach (var file in catalogFiles)
        {
            var text = File.ReadAllText(file);
            Assert.DoesNotContain("Ecommerce.Modules.Inventory.Data", text);
            Assert.DoesNotContain("Ecommerce.Modules.Inventory.Queries", text);
            Assert.DoesNotContain("Ecommerce.Modules.Inventory.Commands", text);
        }
    }

    [Fact]
    public void Catalog_feature_has_no_command_handlers()
    {
        var commandsRoot = Path.Combine(RepoRoot(), "src", "backend", "src", "Ecommerce.Modules.Catalog", "Commands");

        Assert.False(Directory.Exists(commandsRoot));
    }

    [Fact]
    public void Frontend_tests_reference_api_client_instead_of_backend_modules()
    {
        var frontendRoot = Path.Combine(RepoRoot(), "src", "frontend", "storefront");
        if (!Directory.Exists(frontendRoot))
        {
            return;
        }

        var frontendFiles = Directory.EnumerateFiles(frontendRoot, "*.*", SearchOption.AllDirectories)
            .Where(file => file.EndsWith(".ts", StringComparison.OrdinalIgnoreCase) ||
                           file.EndsWith(".tsx", StringComparison.OrdinalIgnoreCase));

        foreach (var file in frontendFiles)
        {
            var text = File.ReadAllText(file);
            Assert.DoesNotContain("Ecommerce.Modules", text);
        }
    }

    private static string RepoRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !Directory.Exists(Path.Combine(directory.FullName, ".specify")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName ?? throw new InvalidOperationException("Could not locate repository root.");
    }
}
