using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using WebApp.Models.Customer;

namespace WebApp.Services
{
    public class CustomerSearchService
    {
        private readonly MVCDBContext _dbContext;

        public CustomerSearchService()
        {
            _dbContext = new MVCDBContext();
        }

        public List<DBCustomerSearchModel> GetCustomerFirstToTen()
        {
            string sql = @"
                SELECT TOP 10 
                    CUST_ID AS ID, 
                    CUST_NM AS Name, 
                    CUST_TYPE AS Type, 
                    TEL_NO AS PhoneNumber, 
                    ZIPCODE AS ZipCode, 
                    ADDRESS AS Address
                FROM MST_CUSTOMER 
                ORDER BY CUST_ID ASC";

            return _dbContext.Database.SqlQuery<DBCustomerSearchModel>(sql).ToList();
        }

        public List<DBCustomerSearchModel> Getcustomer(FormCustomerSearchModel model)
        {
            //順番
            //SELECT    取得する項目
            //FROM      取得元のテーブル
            //WHERE　　 検索条件
            //ORDER BY  並び替え(ソート)
            //OFFSET　　何件目から何件目まで設定

            /*
             * 		検索条件：																					
					顧客マスタ・顧客ID≧[画面・検索条件顧客IDFrom]																		（入力がない場合、検索条件に絞り込まない）
			AND		顧客マスタ・顧客ID≦[画面・検索条件顧客IDTo]																		（入力がない場合、検索条件に絞り込まない）
			AND		顧客マスタ・顧客種別 in ([画面・顧客種別])																		（チェックされた種別がない場合、検索条件に絞り込まない）
			AND		顧客マスタ・削除フラグ＝0																		
																							
			画面・キーワードの処理について																				
				画面入力を空白（全角、半角、タブなど）で区切られた文字列を取得して配列に保存する																			
				配列の各要素を下記のように検索条件に絞り込む																			
					顧客マスタ・顧客名 like '%[要素]%'																		
					OR 顧客マスタ・電話番号（検索用） like '%[要素]%'																		
					OR 顧客マスタ・住所（検索、表示用） like '%[要素]%'																		
				各要素の検索はANDで結合して検索条件に絞り込む																			
																							
				例：　　入力されたキーワード「1234　あいうえ　カキク」である場合																			
				下記のような検索条件になる																			
					AND																		
						(顧客マスタ・顧客名 like '%1234%'																	
						OR 顧客マスタ・電話番号（検索用） like '%1234%'																	
						OR 顧客マスタ・住所（検索、表示用） like '%1234%')																	
					AND																		
						(顧客マスタ・顧客名 like '%あいうえ%'																	
						OR 顧客マスタ・電話番号（検索用） like '%あいうえ%'																	
						OR 顧客マスタ・住所（検索、表示用） like '%あいうえ%')																	
					AND																		
						(顧客マスタ・顧客名 like '%カキク%'																	
						OR 顧客マスタ・電話番号（検索用） like '%カキク%'																	
						OR 顧客マスタ・住所（検索、表示用） like '%カキク%')																	
																							
		        ソート：																					
			        顧客IDソートマークが▲の場合											顧客ID 昇順									
			        顧客IDソートマークが▼の場合											顧客ID 降順									
			        顧客名ソートマークが▲の場合											顧客名 昇順									
			        顧客名ソートマークが▼の場合											顧客名 降順									
			        顧客種別ソートマークが▲の場合											顧客種別 昇順									
			        顧客種別ソートマークが▼の場合											顧客種別 降順									
																							
		        注：削除する時、排他チェックのため、更新時刻を取得して保存することが必要																					
             * 
             */
            string sql = @"
                SELECT
                    CUST_ID AS ID,
                    CUST_NM AS Name,
                    CUST_TYPE AS Type,
                    TEL_NO AS PhoneNumber, 
                    ZIPCODE AS ZipCode, 
                    ADDRESS AS Address
                FROM MST_CUSTOMER
                WHERE 1=1 
            ";

            var parameters = new List<SqlParameter>();  // これをメソッドの最初に追加

            if (!model.CondCustomerIdFromString.IsNullOrEmpty())
            {
                try // CondCustomerIdFromString
                {
                    int idFrom = int.Parse(model.CondCustomerIdFromString);
                    sql += " AND CUST_ID <= @idFrom ";
                    parameters.Add(new SqlParameter("@idFrom", idFrom));  // パラメータ追加
                }
                catch (FormatException ex) { Debug.WriteLine(ex); }
            }

            if(!model.CondCustomerIdToString.IsNullOrEmpty())
            {
                try // CondCustomerIdToString
                {
                    int idTo = int.Parse(model.CondCustomerIdToString);
                    sql += " AND CUST_ID >= @idTo ";
                    parameters.Add(new SqlParameter("@idTo", idTo));
                }
                catch (FormatException ex) { Debug.WriteLine(ex); }
            }

            //chkCustomerTypeの判定
            string CustomerTypeString=null;
            if (model.chkCustomerType0 || model.chkCustomerType1 || model.chkCustomerType2)
            {
                sql += " AND (";

                if (model.chkCustomerType0)
                {
                    sql += " CUST_TYPE = @CustomerType00 OR";
                    parameters.Add(new SqlParameter("@CustomerType00", "00"));
                }
                if (model.chkCustomerType1)
                {
                    sql += " CUST_TYPE = @CustomerType01 OR";
                    parameters.Add(new SqlParameter("@CustomerType01", "01"));
                }
                if (model.chkCustomerType2)
                {
                    sql += " CUST_TYPE = @CustomerType02 OR";
                    parameters.Add(new SqlParameter("@CustomerType02", "02"));
                }

                // 最後のORを削除
                sql = sql.TrimEnd("OR".ToCharArray());
                sql += ")";
            }

            // SQLデバッグ用
            Debug.WriteLine("現在のSQLクエリ: " + sql);
            foreach (var param in parameters)
            {
                Debug.WriteLine($"パラメータ: {param.ParameterName} = {param.Value}");
            }

            return _dbContext.Database.SqlQuery<DBCustomerSearchModel>(sql, parameters.ToArray()).ToList();
        }
    }
}
