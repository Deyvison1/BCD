using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BCD.Domain.Entities;
using BCD.Repository.EntitiesRepository.HistoricoRepository;
using BCD.WebApi.Dtos;
using BCD.WebApi.Services.Exception;

namespace BCD.WebApi.Services.HistoricoServices
{
    public class HistoricoService
    {
        private readonly IMapper _map;
        private readonly IHistoricoRepository _repo;
        public HistoricoService(IMapper map, IHistoricoRepository repo)
        {
            _repo = repo;
            _map = map;
        }
        // LISTAR PELO MES CONTA POUPANCA
        public async Task<HistoricoDto[]> GeteByMesPoupanca(int mes, int agencia, int conta)
        {
            var historicoByMesPoupanca = await _repo.GetByMesPoupancaAsync(mes, agencia, conta);

            var historicoByMesPoupancaDto = _map.Map<HistoricoDto[]>(historicoByMesPoupanca);
            
            return historicoByMesPoupancaDto.ToArray();
        }
        // LISTAR PELO MES CONTA CORRENTE
        public async Task<HistoricoDto[]> GetByMesCorrente(int mes, int agencia, int conta)
        {
            var historicoByMes = await _repo.GetByMesCorrenteAsync(mes, agencia, conta);

            var historicoByMesDto = _map.Map<HistoricoDto[]>(historicoByMes);
            
            return historicoByMesDto;
        }
        // DELETAR
        public async Task<HistoricoDto> Delete(int id) 
        {
            var historico = await _repo.GetByIdAsync(id);
            if(historico == null) throw new NotFoundException("Nenhum extrato encontrado com esse id!");

            _repo.Delete(historico);
            if(await _repo.SaveAsync())
            {
                return _map.Map<HistoricoDto>(historico);
            }
            throw new ArgumentException("Erro ao persitir no banco!");
        }
        // ATUALIZAR
        public async Task<HistoricoDto> Update(HistoricoDto historicoDto) 
        {
            var historico = await _repo.GetByIdAsync(historicoDto.Id);
            if(historico == null) throw new NotFoundException("Nenhum extrato encontrado com esse id!");

            _map.Map(historico, historicoDto);
            _repo.Update(historico);
            if(await _repo.SaveAsync())
            {
                return _map.Map<HistoricoDto>(historico);
            }
            throw new ArgumentException("Erro ao persitir no banco!");
        }
        // ADICIONAR
        public async Task<HistoricoDto> Add(HistoricoDto historicoDto) 
        {
            var historico = _map.Map<Historico>(historicoDto);

            _repo.Add(historico);
            if(await _repo.SaveAsync())
            {
                return _map.Map<HistoricoDto>(historico);
            }
            throw new ArgumentException("Erro ao persitir no banco!");
        }
        // LISTAR POR ID
        public async Task<HistoricoDto[]> GetValorOrDescricaoOrConta(string search) 
        {
            var historico = await _repo.GetBySearch(search);

            var historicoDto = _map.Map<HistoricoDto[]>(historico);

            return historicoDto.ToArray();
        }
        // LISTAR POR ID
        public async Task<HistoricoDto> GetById(int id) 
        {
            var historico = await _repo.GetByIdAsync(id);
            if(historico == null) throw new NotFoundException("Nenhum extrato encontrado!");

            var historicoDto = _map.Map<HistoricoDto>(historico);

            return historicoDto;
        }
        // LISTAR TODOS
        public async Task<HistoricoDto[]> GetAll() 
        {
            var historico = await _repo.GetAllAsync();

            var historicoDto = _map.Map<HistoricoDto[]>(historico);

            return historicoDto.ToArray();
        }

    }
}