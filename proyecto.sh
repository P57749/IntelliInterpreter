#!/bin/bash

# Variables
TEX_REPORT="informe.tex" # Nombre del archivo LaTeX del informe
TEX_SLIDES="presentacion.tex" # Nombre del archivo LaTeX de las presentaciones
PDF_REPORT="${TEX_REPORT%.tex}.pdf" # Nombre del archivo PDF del informe generado
PDF_SLIDES="${TEX_SLIDES%.tex}.pdf" # Nombre del archivo PDF de las presentaciones generado
CONSOLE_PROJECT="Interprete" # Nombre del proyecto de la aplicación de consola en C#
VIEWER_CMD_DEFAULT="xdg-open" # Comando por defecto para visualizar archivos PDF

# Función para compilar un archivo LaTeX y generar el PDF
function compile_pdf() {
    latexmk -pdf "$1"
}

# Opción: run - Ejecutar la aplicación de consola en C#
function run_project() {
    # Comandos para compilar y ejecutar la aplicación de consola
    echo "Compilando y ejecutando la aplicación de consola..."
    cd "$CONSOLE_PROJECT"
    dotnet build
    dotnet run
}

# Opción: report - Compilar y generar el PDF del informe
function generate_report_pdf() {
    echo "Compilando el informe LaTeX..."
    compile_pdf "$TEX_REPORT"
}

# Opción: slides - Compilar y generar el PDF de las presentaciones
function generate_slides_pdf() {
    echo "Compilando las presentaciones LaTeX..."
    compile_pdf "$TEX_SLIDES"
}

# Opción: show_report - Visualizar el informe PDF
function show_report_pdf() {
    if [ ! -f "$PDF_REPORT" ]; then
        echo "Generando el informe PDF..."
        generate_report_pdf
    fi

    echo "Mostrando el informe..."
    "$VIEWER_CMD" "$PDF_REPORT"
}

# Opción: show_slides - Visualizar las presentaciones PDF
function show_slides_pdf() {
    if [ ! -f "$PDF_SLIDES" ]; then
        echo "Generando las presentaciones PDF..."
        generate_slides_pdf
    fi

    echo "Mostrando las presentaciones..."
    "$VIEWER_CMD" "$PDF_SLIDES"
}

# Opción: clean - Eliminar ficheros auxiliares generados en la compilación
function clean_project() {
    echo "Limpiando ficheros auxiliares..."
    latexmk -c
    rm -f *.snm *.nav
    cd "$CONSOLE_PROJECT"
    dotnet clean
}

# Opciones del script
case "$1" in
    "run")
        run_project
        ;;
    "report")
        generate_report_pdf
        ;;
    "slides")
        generate_slides_pdf
        ;;
    "show_report")
        VIEWER_CMD=${2:-$VIEWER_CMD_DEFAULT}
        show_report_pdf
        ;;
    "show_slides")
        VIEWER_CMD=${2:-$VIEWER_CMD_DEFAULT}
        show_slides_pdf
        ;;
    "clean")
        clean_project
        ;;
    *)
        echo "Uso: $0 {run|report|slides|show_report|show_slides|clean}"
        exit 1
        ;;
esac
