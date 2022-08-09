﻿using CursoOnline.Dominio._Base;
using System;

namespace CursoOnline.Dominio.Domain
{
    public class ArmazenadorDeAluno
    {
        #region Atributos
        private readonly IAlunoRepositorio _alunoRepositorio;
        #endregion

        #region Construtores
        public ArmazenadorDeAluno(IAlunoRepositorio alunoRepositorio)
        {
            _alunoRepositorio = alunoRepositorio;
        }

        public void Armazenar(AlunoDto alunoDto)
        {
            var alunoJaCadastrado = _alunoRepositorio.ObterPeloCpf(alunoDto.Cpf);

            ValidadorDeRegra.Novo()
                .Quando((alunoJaCadastrado != null && alunoJaCadastrado.Id != alunoDto.Id), Resource.CpfJaCadastrado)
                .Quando(!Enum.TryParse<PublicoAlvo>(alunoDto.PublicoAlvo, out PublicoAlvo publicoAlvo), Resource.PublicoAlvoInvalido)
                .DispararExcecaoSeExistir();

            if (alunoDto.Id == 0)
            {
                var aluno = new Aluno(alunoDto.Nome, alunoDto.Email, alunoDto.Cpf, publicoAlvo);
                _alunoRepositorio.Adicionar(aluno);
            }
            else
            {
                var aluno = _alunoRepositorio.ObterPorId(alunoDto.Id);
                aluno.AlterarNome(alunoDto.Nome);
            }
        }
        #endregion
    }
}
