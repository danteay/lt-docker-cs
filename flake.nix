{
  description = "Ambiente de desarrollo docker netcore";

  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs?ref=nixos-unstable";
    flake-utils.url = "github:numtide/flake-utils";
  };

  outputs = { self, nixpkgs, flake-utils }:
    flake-utils.lib.eachDefaultSystem(system:
      let
        pkgs = nixpkgs.legacyPackages.${system};
      in
      {
        devShell = pkgs.mkShell {
          buildInputs = with pkgs; [
            dotnetCorePackages.sdk_9_0-bin

            docker
            docker-compose

            openssl
            postgresql # to obtain libpq-dev
            
            gnumake
            terraform
            awscli2
            tflint
            commitizen
            husky
          ];
        };
      }
    );
}