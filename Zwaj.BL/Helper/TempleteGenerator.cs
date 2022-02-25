using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwaj.BL.DTOs;
using Zwaj.BL.Interfaces;

namespace Zwaj.BL.Helper
{
    public class TempleteGenerator
    {
        private readonly IMapper _mapper;
        private readonly IZwajRep _repo;

        public TempleteGenerator(IZwajRep repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;

        }

        public string GetHTMLStringForUser(string userId)
        {
            //exception of Global Query Filter we use false
            var user = _repo.GetUser(userId).Result;
            var userToReturn = _mapper.Map<UserForDetailsDTO>(user);

            var likers = _repo.GetLikersOrLikees(userId, "likers").Result;
            var likees = _repo.GetLikersOrLikees(userId, "likees").Result;
            var likersCount = likers.Count;
            var likeesCount = likees.Count;


            var sb = new StringBuilder();

            sb.Append(@"
                        <html dir='rtl'>
                            <head>
                            </head>
                            <body>
                                <div class='page-header'><h2 class='header-container'>بطاقة " + userToReturn.Name + @"</h2></div>
                                                             
                                <div class='card-data'>
                                 <img src='" + userToReturn.PhotoUrl + @"'>
                                <table style='display:inline;width: 50%;height: 300px;'>
                                <div>
                                <tr>
                                <td>الإسم</td>
                                    <td>" + userToReturn.Name + @"</td>
                                </tr>
                                <tr>
                                    <td>العمر</td>
                                    <td>" + userToReturn.DateOfBirth + @"</td>
                                </tr>    
                                <tr>
                                    <td>البلد</td>
                                    <td>" + userToReturn.Country + @"</td>
                                </tr>    
                                <tr>
                                    <td>تاريخ الإشتراك</td>
                                    <td>" + userToReturn.Created.ToShortDateString() + @"</td>
                                </tr> 
                                </div>   
                              </table>
                                </div>
                                <div class='page-header'><h2 class='header-container'>المعجبين &nbsp;&nbsp;[" + likersCount + @"]</h2></div>
                                <table align='center'>
                                    <tr>
                                        <th>الإسم</th>
                                        <th>تاريخ الإشتراك</th>
                                        <th>العمر</th>
                                        <th>البلد</th>
                                    </tr>");

            foreach (var liker in likers)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                  </tr>", liker.Name, liker.Created.ToString(), liker.DateOfBirth, liker.Country);
            }

            sb.Append(@"
                                </table>
                                <div class='page-header'><h2 class='header-container'>المعجب بهم  &nbsp;&nbsp;[" + likeesCount + @"] </h2></div>
                                <table align='center'>
                                <tr>
                                 <th>الإسم</th>
                                        <th>تاريخ الإشتراك</th>
                                        <th>العمر</th>
                                        <th>البلد</th>
                                </tr>");
            foreach (var likee in likees)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                  </tr>", likee.Name, likee.Created, likee.DateOfBirth, likee.Country);
            }

            sb.Append(@"     </table>                   
                            </body>
                        </html>");

            return sb.ToString();
        }


    }
}
