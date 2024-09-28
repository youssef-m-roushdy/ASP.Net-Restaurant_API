using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Dtos.BranchDto;
using efcoremongodb.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace efcoremongodb.Controllers
{
    [Route("/api/branch")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branch;
        public BranchController(IBranchService branch)
        {
            _branch = branch;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var branches = await _branch.GetAllBranchs();
            return Ok(branches);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var branch = await _branch.GetBranchById(id);
            if(branch == null)
                return NotFound();

            return Ok(branch);
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBranchDto branch)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            var createdBranch = await _branch.AddBranch(branch);
            return CreatedAtAction(nameof(GetById), new{id = createdBranch.Id}, createdBranch);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id,[FromBody] UpdateBranchDto branch)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            var updatedBranch = await _branch.EditBranch(id, branch);
            return Ok(updatedBranch);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            await _branch.DeleteBranch(id);
            return NoContent();
        }
    }
}